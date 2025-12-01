using CSharp_OpenCV_NET.ViewModels;
using CSharp_OpenCV_NET.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CSharp_OpenCV_NET.Services
{
    /// <summary>
    /// 导航服务的具体实现，负责"打开窗口"或"弹出对话框"
    /// 采用依赖注入（DI）方式获取窗口实例，避免直接 new，利于单元测试与生命周期管理
    /// </summary>
    public class NavigationService : INavigationService
    {
        // DI 容器，运行时里所有服务/窗口都已注册在内
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 通过构造函数注入 IServiceProvider，供后续解析窗口实例。
        /// </summary>
        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        /// <summary>
        /// 打开一个非模态窗口（导航到新页面）。
        /// <typeparamref name="T"/> 必须是 Window 或其子类，且需要提前在 DI 容器中注册。
        /// </summary>
        /// <typeparam name="T">要打开的窗口类型。</typeparam>
        public void NavigateTo<T>() where T : Window
        {
            // 从 DI 容器解析窗口实例；如果未注册，GetService 返回 null
            var window = _serviceProvider.GetService<T>();
            // ?. 运算符防止未注册时抛出 NullReferenceException
            window?.Show();
        }
        /// <summary>
        /// 以模态对话框形式打开窗口（阻塞父窗口，直到对话框关闭）。
        /// <typeparamref name="T"/> 必须是 Window 或其子类，且需要提前在 DI 容器中注册。
        /// </summary>
        /// <typeparam name="T">要弹出的对话框类型。</typeparam>
        public void ShowDialog<T>() where T : Window
        {
            var window = _serviceProvider.GetService<T>();
            // ShowDialog() 会返回 bool?，这里不需要返回值
            window?.ShowDialog();
        }

        /// <summary>
        /// 打开一个非模态窗口并自动设置 DataContext
        /// </summary>
        public void NavigateTo<TWindow, TViewModel>()
            where TWindow : Window
            where TViewModel : class
        {
            var window = _serviceProvider.GetRequiredService<TWindow>();
            var viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            window.DataContext = viewModel;
            window.Show();
        }

        /// <summary>
        /// 以模态对话框形式打开窗口并自动设置 DataContext
        /// </summary>
        public void ShowDialog<TWindow, TViewModel>()
            where TWindow : Window
            where TViewModel : class
        {
            var window = _serviceProvider.GetRequiredService<TWindow>();
            var viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            window.DataContext = viewModel;
            window.ShowDialog();
        }


        #region 便携窗口导航方法
        /// <summary>
        /// 显示用户管理对话框（便捷方法）
        /// </summary>
        public void ShowUserManagerDialog()
        {
            ShowDialog<UserManagerWindow, UserManagerViewModel>();
        }

        /// <summary>
        /// 显示用户登录对话框（便捷方法）
        /// </summary>
        public void ShowUserLoginDialog()
        {
            ShowDialog<UserLoginWindow, UserLoginViewModel>();
        }
        #endregion
    }
}
