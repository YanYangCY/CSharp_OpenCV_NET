using CommunityToolkit.Mvvm.Messaging;
using CSharp_OpenCV_NET.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CSharp_OpenCV_NET.Views
{
    /// <summary>
    /// SplashWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SplashWindow : Window, IRecipient<string>
    {
        public SplashWindow()
        {
            InitializeComponent();

            // 1. 注册（用 WeakReferenceMessenger），收到"CloseSplashWindow"字符串就执行关闭操作
            WeakReferenceMessenger.Default.Register<string, string>(this, "CloseSplashWindow");
            // 2. 卸载时注销，防止内存泄露
            Unloaded += (_, _) => WeakReferenceMessenger.Default.UnregisterAll(this);

        }

        // 实现接口，收到消息即关闭
        public void Receive(string msg) => Close();
    }
}
