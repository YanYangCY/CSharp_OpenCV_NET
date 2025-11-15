using log4net.Config;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace CSharp_OpenCV_NET.Log
{
    public static class MTLogger
    {
        /// <summary>
        /// 全局静态门面：任何地方 MTLogger.Info(...) 就能写日志
        /// 1. 自动加载 log4net.config
        /// 2. 自动把“非 UI 线程”调用封送到 UI 线程（防止 RichTextBox 跨线程异常）
        /// 3. 同时支持文件、控制台、彩色富文本框
        /// </summary>
        static MTLogger()
        {
            var cfg = new FileInfo(@"Log\log4net.config");
            // 如果文件真丢了就抛异常，避免静默失败
            if (!cfg.Exists)
                throw new FileNotFoundException("log4net.config 不存在！", cfg.FullName);
            // ConfigureAndWatch = 启动后改配置无需重启程序
            XmlConfigurator.ConfigureAndWatch(cfg);
        }

        // 公共 logger
        private static readonly ILog Logger = LogManager.GetLogger("MTLogger");

        /* ---------- 下面 5 个方法统一 UI 线程 ---------- */
        public static void Debug(string message)
            => InvokeOnUI(() => Logger.Debug(message));

        public static void Info(string message)
            => InvokeOnUI(() => Logger.Info(message));

        public static void Warn(string message)
            => InvokeOnUI(() => Logger.Warn(message));

        public static void Error(string message)
            => InvokeOnUI(() => Logger.Error(message));

        public static void Fatal(string message)
            => InvokeOnUI(() => Logger.Fatal(message));

        /* ---------- 统一封装的 UI 调度 ---------- */
        private static void InvokeOnUI(System.Action action)
        {
            var dispatcher = App.Current?.Dispatcher;
            if (dispatcher is null) { action(); return; }

            if (dispatcher.CheckAccess())
                action();
            else
                dispatcher.BeginInvoke(action, DispatcherPriority.Normal);
        }
    }
}
