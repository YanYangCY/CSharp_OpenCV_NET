using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSharp_OpenCV_NET.Models;
using CSharp_OpenCV_NET.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_OpenCV_NET.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        // 声明一个私有只读字段，用于存储AppStatusModel的引用
        private readonly AppStatusModel _appStatus;
        
        private readonly IServiceProvider _serviceProvider;

        // 通过构造函数注入AppStatusModel
        // 这个appStatus参数是由DI容器自动提供的
        public MainViewModel(AppStatusModel appStatus, IServiceProvider serviceProvider)
        {
            // 将传入的appStatus赋值给私有字段_appStatus
            _appStatus = appStatus;
            _serviceProvider = serviceProvider;
        }

        // 暴露AppStatus属性供View绑定
        // 这是一个只读属性，返回我们存储的_appStatus
        public AppStatusModel AppStatus => _appStatus;

        #region 模式切换命令
        [RelayCommand]
        private void SwitchRunMode()
        {
            // 切换运行状态
            //_appStatus.IsOnline = !_appStatus.IsOnline;
            _appStatus.NumAdd++;
            //bool sb = _appStatus.IsOnline;
        }
        #endregion

        #region 用户登录命令
        [RelayCommand]
        private void UserLogin()
        {
            // 从容器拿窗口
            var loginWindow = _serviceProvider.GetRequiredService<UserLoginWindow>();
            //loginWindow.Owner = App.Current.MainWindow;   // 可选，指定父窗口
            loginWindow.ShowDialog();
        }
        #endregion

        [RelayCommand]
        private void ButtonAddNum()
        {
            _appStatus.NumAdd++;
        }
    }
}
