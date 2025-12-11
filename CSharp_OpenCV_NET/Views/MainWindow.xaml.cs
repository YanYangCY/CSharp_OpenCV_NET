using CSharp_OpenCV_NET.Communicate.TCP;
using CSharp_OpenCV_NET.Views.SubViews;
using Microsoft.Extensions.DependencyInjection;
using SuperSocket.Server.Abstractions;
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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        // 关闭窗口时停止服务器
        //SocketServer _server = new SocketServer();
        //protected override async void OnClosed(EventArgs e)
        //{
        //    base.OnClosed(e);

        //    if (_server != null)
        //    {
        //        await _server.Stop();
        //    }
        //}
    }
}
