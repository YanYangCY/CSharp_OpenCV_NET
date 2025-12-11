using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CSharp_OpenCV_NET.Communicate.TCP;
using CSharp_OpenCV_NET.Log;
using CSharp_OpenCV_NET.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;


namespace CSharp_OpenCV_NET.ViewModels
{
    public partial class SplashViewModel
    {
        private readonly SocketServer _socketServer;

        // 通过构造函数注入单例
        public SplashViewModel(SocketServer socketServer)
        {
            _socketServer = socketServer;
        }

        [RelayCommand]
        private void WindowLoaded()
        {
            Task.Run(AppStartUp);
        }
        private void AppStartUp()
        {

            // 初始化新建文件夹
            CSharp_OpenCV_NET.Configuration.AppPaths.EnsureDirectoriesExist();

            // 创建用户模型实例
            var userModel = new UserModel();

            MTLogger.Info("软件启动");

            // 模拟耗时
            Thread.Sleep(1500);
            #region 通讯启动
            App.Current.Dispatcher.BeginInvoke(() =>
            {
                //var _server = new SocketServer();
                // 启动服务器（异步启动，不阻塞UI）
                Task.Run(async () => await _socketServer.Start());
            });
            Thread.Sleep(200);
            #endregion
            // UI线程异步执行
            App.Current.Dispatcher.BeginInvoke(() =>
            {
                // 消息内容+令牌
                WeakReferenceMessenger.Default.Send<string,string>("close", "CloseSplashWindow");
            });
            Thread.Sleep(200);
        }
    }
}
