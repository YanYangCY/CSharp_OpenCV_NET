using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CSharp_OpenCV_NET.Services
{
    /// <summary>
    /// 导航服务接口
    /// 提供多种窗口打开方式，支持模态和非模态窗口，指定设置了DataContext
    /// </summary>
    public interface INavigationService
    {
        // 打开一个新窗口（T 类型），并导航到它
        void NavigateTo<T>() where T : Window;
        // 以对话框的形式打开窗口（模态窗口）
        void ShowDialog<T>() where T : Window;

        // 带 ViewModel 设置的方法
        void NavigateTo<TWindow, TViewModel>()
            where TWindow : Window
            where TViewModel : class;

        void ShowDialog<TWindow, TViewModel>()
            where TWindow : Window
            where TViewModel : class;

        // 打开非初始化窗口及主界面窗口的便携导航方法
        void ShowUserManagerDialog();
        void ShowUserLoginDialog();
    }
}
