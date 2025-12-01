using CommunityToolkit.Mvvm.Messaging;
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
    /// UserLoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UserLoginWindow : Window, IRecipient<string>
    {
        public UserLoginWindow()
        {
            InitializeComponent();

            // 1. 注册（用 WeakReferenceMessenger），收到"CloseSplashWindow"字符串就执行关闭操作
            WeakReferenceMessenger.Default.Register<string, string>(this, "CloseLoginWindow");
            // 2. 卸载时注销，防止内存泄露
            Unloaded += (_, _) => WeakReferenceMessenger.Default.UnregisterAll(this);
        }

        // 实现接口，收到消息即关闭
        public void Receive(string msg) => Close();
    }
}
