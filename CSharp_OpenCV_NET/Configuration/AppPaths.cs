using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_OpenCV_NET.Configuration
{
    // 配置全局静态路径
    public static class AppPaths
    {
        // 应用根目录，exe所在文件夹路径（通常为bin/Debug 或安装目录）:返回的字符串末尾带反斜杠
        public static string AppRoot => AppDomain.CurrentDomain.BaseDirectory;

        // 日志目录
        public static string LogPath => Path.Combine(AppRoot, "Logs");

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
