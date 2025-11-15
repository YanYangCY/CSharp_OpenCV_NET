using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using CSharp_OpenCV_NET.Views;
using CSharp_OpenCV_NET.Services;
using CSharp_OpenCV_NET.ViewModels;

//using CSharp_OpenCV_NET.ViewModels;

namespace CSharp_OpenCV_NET
{
    /// <summary>
    /// 应用程序入口类，负责依赖注入容器的配置与启动流程控制。
    /// 使用 Microsoft.Extensions.DependencyInjection 实现服务注册与解析，
    /// 遵循 MVVM 架构，实现 View 与 ViewModel 的解耦。
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 全局静态的服务提供者，用于在应用程序任意位置解析已注册的服务（如 ViewModel、窗口等）。
        /// 注意：仅适用于简单场景；复杂应用建议通过构造函数注入避免全局状态。
        /// IServiceProvider: 这是一个服务提供者接口，用于获取已注册的服务实例。
        /// </summary>
        public static IServiceProvider? ServiceProvider { get; private set; }
        /// <summary>
        /// 应用程序构造函数，在 WPF 启动前执行。
        /// 此处用于配置依赖注入（DI）容器，注册所有需要的服务、视图（View）和视图模型（ViewModel）。
        /// </summary>
        public App()
        {
            // ========== 注册应用程序组件 ==========
            // 创建一个服务集合，用于注册服务（包括ViewModels和Views）-配置依赖注入容器
            var services = new ServiceCollection();
            /* 1. 先注册所有服务、VM、窗体 */
            // 把"导航服务"注册到 .NET 的依赖注入（DI）容器
            //services.AddSingleton<INavigationService, NavigationService>();
            // 注册ViewModels,AddTransient每次请求该服务时都会创建一个新的实例
            // 注册Views
            services.AddTransient<MainWindow>();
            services.AddTransient<SplashWindow>();
            // 注册ViewModel
            services.AddTransient<SplashViewModel>();

            // ========== 构建服务提供者 ==========
            // 构建 IServiceProvider 实例，并赋值给静态属性，供后续启动流程使用
            ServiceProvider = services.BuildServiceProvider();      
        }

        /// <summary>
        /// 应用程序启动时调用，控制启动顺序：
        /// 1. 显示启动屏（模态对话框）
        /// 2. 执行后台初始化任务（由 SplashViewModel 触发）
        /// 3. 关闭启动屏后显示主窗口
        /// </summary>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // 从容器中获取主窗口（暂不显示）
            var mainWindow = ServiceProvider?.GetService<MainWindow>();

            /* 启动屏（DI + DataContext） */
            var splash = ServiceProvider!.GetRequiredService<SplashWindow>();
            splash.DataContext = ServiceProvider!.GetRequiredService<SplashViewModel>();
            // 以模态方式显示启动屏（阻塞主线程，直到关闭）等它自己发消息关闭
            splash.ShowDialog();

            // 显示主窗口
            mainWindow?.Show();
        }

    }


}
