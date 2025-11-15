using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_OpenCV_NET.Configuration
{
    /// <summary>
    /// 提供应用程序中常用的全局路径定义，并确保必要目录在启动时存在。
    /// 所有路径基于应用程序的可执行文件（.exe）所在目录（即 AppDomain.CurrentDomain.BaseDirectory）
    /// </summary>
    public static class AppPaths
    {
        // 应用根目录，exe所在文件夹路径（通常为bin/Debug 或安装目录）:返回的字符串末尾带反斜杠
        public static string AppRoot => AppDomain.CurrentDomain.BaseDirectory;

        // 日志目录
        public static string LogPath => Path.Combine(AppRoot, "Logs");


        /// <summary>
        /// 确保应用程序运行所需的目录结构已创建。
        /// 如果目录不存在，则自动创建；如果已存在，则不执行任何操作。
        /// 当前包括：
        /// - 日志目录（<see cref="LogPath"/>）
        /// 
        /// 此方法通常在应用程序启动阶段（如 Splash 屏初始化时）调用，
        /// 避免后续写日志或保存文件时因目录缺失而抛出异常。
        /// </summary>
        public static void EnsureDirectoriesExist()
        {
            var dirs = new[]
            {
                LogPath
            };
            foreach (var dir in dirs)
            {
                Directory.CreateDirectory(dir);
            }
        }
    }
}
