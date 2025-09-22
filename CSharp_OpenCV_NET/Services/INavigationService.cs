using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CSharp_OpenCV_NET.Services
{
    public interface INavigationService
    {
        // 打开一个新窗口（T 类型），并导航到它
        void NavigateTo<T>() where T : Window;
        // 以对话框的形式打开窗口（模态窗口）
        void ShowDialog<T>() where T : Window;
    }
}
