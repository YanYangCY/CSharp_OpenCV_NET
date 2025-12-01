using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CSharp_OpenCV_NET.Log;
using CSharp_OpenCV_NET.Models;
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

            // UI线程异步执行
            App.Current.Dispatcher.BeginInvoke(() =>
            {
                // 消息内容+令牌
                WeakReferenceMessenger.Default.Send<string,string>("close", "CloseSplashWindow");
            });
        }
    }
}
