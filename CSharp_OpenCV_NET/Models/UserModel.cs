using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharp_OpenCV_NET.Models.Attribute;
using CSharp_OpenCV_NET.Configuration;
using System.IO;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using CSharp_OpenCV_NET.Log;

namespace CSharp_OpenCV_NET.Models
{
    // 创建结构体用于返回捕获时间
    [StructLayout(LayoutKind.Sequential)]
    struct LASTINPUTINFO
    {
        // 设置结构体块容量
        [MarshalAs(UnmanagedType.U4)]
        public int cbSize;

        // 自系统启动以来最后一次输入事件的时间戳（单位：毫秒）
        [MarshalAs(UnmanagedType.U4)]
        public uint dwTime;
    }

    public partial class UserModel : ObservableObject
    {
        #region 字段定义
        // 默认内置用户列表（用于首次运行或配置损坏时恢复）
        private List<User> _defaultUsers;
        // 用户数据文件路径
        private readonly string _userFilePath =Path.Combine(AppPaths.ConfigPath, "Users.json");
        // 会话超时定时器（用于高级用户无操作固定秒数后自动降权）
        private DispatcherTimer timer = new DispatcherTimer();
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserModel() 
        {
            _defaultUsers = new List<User>()
            {
                new User(){Name = "Administrator", Password = EncryptPassword("admin123"), Level = 3 },
                new User(){Name = "Engineer",      Password = EncryptPassword("engineer123"), Level = 2 },
                new User(){Name = "Operator",      Password = EncryptPassword("operator123"), Level = 1 },
            };

            // 加载用户配置
            LoadUserConfig();
        }

        #region 公共属性
        // 用于在 UI（如 ComboBox）中显示所有可用用户
        [ObservableProperty]
        private List<User> _userList = [];

        // 用于向登录界面反馈错误信息
        [ObservableProperty]
        private string _errorMessage = string.Empty;

        // UI上选中的用户名字符串
        [ObservableProperty]
        private string _selectedUser  = string.Empty;
        // 当前已登录的用户
        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                // 安全清理：停止并取消旧定时器订阅，防止内存泄漏
                timer.Stop();
                timer.Tick -= Timer_Tick;

                // 更新当前用户
                _currentUser = value;

                // 如果是高级用户（Level > 1），启动新定时器
                if (_currentUser?.Level > 1)
                {
                    timer.Interval = TimeSpan.FromSeconds(120);
                    timer.Tick += Timer_Tick;
                    timer.Start();
                }
            }
        }
        #endregion

        #region 加载用户文件
        /// <summary>
        /// 从 Users.json 文件加载用户列表
        /// - 若文件不存在 → 创建默认用户
        /// - 若文件损坏/为空 → 回退到默认用户
        /// </summary>
        /// <returns>是否成功加载</returns>
        public bool LoadUserConfig()
        {
            try
            {
                // 如果JSON文件不存在，创建默认用户
                if (!File.Exists(_userFilePath))
                {
                    return CreateDefaultUser(); // 文件不存在，创建默认配置
                }
                // 读取JSON文件
                string jsonString = File.ReadAllText(_userFilePath);
                // 反序列化JSON到用户列表
                var users = JsonConvert.DeserializeObject<List<User>>(jsonString);
                // 若反序列化结果为空或无效
                if (users == null || users.Count == 0)
                {
                    ErrorMessage = "用户文件为空，将使用默认用户";
                    return CreateDefaultUser();
                }
                // 成功加载，更新 UserList（触发 UI 刷新）
                UserList = users;
                return true;
            }
            catch 
            {
                return CreateDefaultUser();
            }
        }
        #endregion
        #region 创建默认用户
        /// <summary>
        /// 创建默认用户并保存到磁盘
        /// </summary>
        /// <returns>是否保存成功</returns>
        private bool CreateDefaultUser()
        {
            UserList = new List<User>(_defaultUsers);
            return SaveUserConfig();
        }
        #endregion
        #region 保存用户信息 - JSON版本
        /// <summary>
        /// 将当前 UserList 保存为 Users.json
        /// - 格式美化（Indented）
        /// - 忽略空值
        /// </summary>
        /// <returns>是否保存成功</returns>
        public bool SaveUserConfig()
        {
            try
            {
                // 判断目录是否存在
                /* 这里应该不需要 */
                // 配置JSON序列号设置
                var settings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,   // 美化输出，便于阅读
                    NullValueHandling = NullValueHandling.Ignore,   // 忽略空值
                };
                // 将用户列表序列化为JSON字符串
                string jsonString = JsonConvert.SerializeObject(UserList, settings);
                // 写入文件
                File.WriteAllText(_userFilePath, jsonString);

                return true;
            }
            catch (Exception e)
            {
                MTLogger.Error("用户信息保存失败！");
                return false;
            }
        }
        #endregion

        #region 定时切换用户权限
        /// <summary>
        /// 获取系统最后一次输入（键盘/鼠标）距今的毫秒数
        /// 利用 Windows API: GetLastInputInfo
        /// </summary>
        /// <returns>无操作时间（毫秒），失败返回 0</returns>
        private static long GetLastInputTime()
        {
            LASTINPUTINFO vLastInputInfo = new LASTINPUTINFO();
            vLastInputInfo.cbSize = Marshal.SizeOf(vLastInputInfo);

            // 捕获时间
            if (!GetLastInputInfo(ref vLastInputInfo))
                return 0;
            else
                return Environment.TickCount - (long)vLastInputInfo.dwTime;
        }
        // 导入 Windows API 函数：获取最后一次输入时间
        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
        /// <summary>
        /// 定时器触发事件：检查是否超时
        /// - 若当前用户为高级用户（Level > 1）
        /// - 且无操作时间 > 120 秒
        /// → 自动切换为 Operator（Level = 1）
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (CurrentUser.Level > 1 && GetLastInputTime() > 120 * 1000)
            {
                CurrentUser = _userList.FirstOrDefault(user => user.Level == 1);
                timer.Stop();
            }
        }
        #endregion

        #region 验证用户密码
        // <summary>
        /// 验证用户名和密码
        /// - 查找用户
        /// - 比对加密后的密码
        /// - 验证成功则设置 CurrentUser
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">明文密码</param>
        /// <returns>是否验证成功</returns>
        public bool Validation(string userName, string password)
        {
            var user = UserList.FirstOrDefault(u => u.Name == userName);
            if (user == null)
            {
                ErrorMessage = "用户不存在";
                return false;
            }

            string encryptedPassword = EncryptPassword(password);
            if (user.Password != encryptedPassword)
            {
                ErrorMessage = "密码错误";
                return false;
            }
            // 验证成功：设置当前用户（会自动启停定时器）
            CurrentUser = user;
            ErrorMessage = string.Empty;
            return true;
        }
        #endregion

        #region 修改用户密码
        public bool ModifyUserPassword(string userName, string newPassword)
        {
            var user = UserList.FirstOrDefault(u => u.Name == userName);
            if (user == null)
                return false;
            // 加密新密码并更新
            string newpass = EncryptPassword(newPassword);
            user.Password = newpass;

            return SaveUserConfig();
        }
        #endregion

        #region MD5方式加密密码
        private string EncryptPassword(string password)
        {
            using var md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
        }
        #endregion

    }
}
