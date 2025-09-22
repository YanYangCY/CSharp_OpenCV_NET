using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using CSharp_OpenCV_NET.Views;
using CSharp_OpenCV_NET.Services;

//using CSharp_OpenCV_NET.ViewModels;

namespace CSharp_OpenCV_NET
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // IServiceProvider: 这是一个服务提供者接口，用于获取已注册的服务实例。
        // ServiceCollection: 用于注册应用程序中所有的服务（包括ViewModels、Views和其他服务）
        public static IServiceProvider? ServiceProvider { get; private set; }

        public App()
        {
            // 创建一个服务集合，用于注册服务（包括ViewModels和Views）-配置依赖注入容器
            var services = new ServiceCollection();
            // 注册ViewModels,AddTransient每次请求该服务时都会创建一个新的实例
            //services.AddTransient<UserViewModel>();
            // 注册Views
            services.AddTransient<MainWindow>();

            // 构建ServiceProvider并赋值给静态属性，以便在应用程序中任何地方都可以使用
            ServiceProvider = services.BuildServiceProvider();

            // 把"导航服务"注册到 .NET 的依赖注入（DI）容器
            services.AddSingleton<INavigationService, NavigationService>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // / 从容器中获取主窗口并显示
            var mainWindow = ServiceProvider?.GetService<MainWindow>();
            mainWindow?.Show();
        }

    }


}
