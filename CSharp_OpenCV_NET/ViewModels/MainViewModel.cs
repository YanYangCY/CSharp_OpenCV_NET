using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSharp_OpenCV_NET.Models;
using CSharp_OpenCV_NET.Services;
using CSharp_OpenCV_NET.Communicate.TCP;
using CSharp_OpenCV_NET.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.Server.Abstractions.Session;

namespace CSharp_OpenCV_NET.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        // 声明一个私有只读字段，用于存储AppStatusModel的引用
        private readonly AppStatusModel _appStatus;

        private readonly INavigationService _navigationService;

        private readonly SocketServer _socketServer;  // 单例实例

        // 通过构造函数注入AppStatusModel
        // 这个appStatus参数是由DI容器自动提供的
        //public MainViewModel(AppStatusModel appStatus, IServiceProvider serviceProvider)
        public MainViewModel(AppStatusModel appStatus, INavigationService navigationService, SocketServer socketServer)
        {
            // 将传入的appStatus赋值给私有字段_appStatus
            _appStatus = appStatus;
            //_serviceProvider = serviceProvider;
            _navigationService = navigationService;
            _socketServer = socketServer;
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
            //var loginWindow = _serviceProvider.GetRequiredService<UserManagerWindow>();
            //loginWindow.DataContext = _serviceProvider.GetRequiredService<UserManagerViewModel>();
            //loginWindow.Owner = App.Current.MainWindow;   // 可选，指定父窗口
            //loginWindow.ShowDialog();
            //_navigationService.ShowUserManagerDialog();
            _navigationService.ShowUserLoginDialog();
        }
        #endregion

        [RelayCommand]
        private void ButtonAddNum()
        {
            _appStatus.NumAdd++;
        }
        [RelayCommand]
        private async Task TcpSendTest()
        {
            try
            {
                // 并且有一个要发送的消息内容，比如"Test Message"
                byte[] data = Encoding.UTF8.GetBytes("Test Message\n");
                await _socketServer.SendToAllAsync(data);

            }
            catch (Exception ex)
            {
            }
        }
    }
}
