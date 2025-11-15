using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CSharp_OpenCV_NET.Log
{
    // 默认调色板实现：级别→颜色 对照表
    internal class CustomLogFormatter : ILogFormatter
    {
        // 复用 StringBuilder，避免每行日志 new 一次
        private StringBuilder stringBuilder = new StringBuilder();

        /// <summary>
        /// 定义各个日志级别对应的输出文字颜色
        /// </summary>
        public Dictionary<Level, Brush> LogLevelToTextForegroundBrush { get; } = new Dictionary<Level, Brush>()
        {
            { Level.Info,  Brushes.White }, // 信息
            { Level.Debug, Brushes.Gray },  // 调试
            { Level.Warn,  Brushes.Orange },// 警告
            { Level.Error, Brushes.Red},    // 错误
            { Level.Fatal, Brushes.DarkRed }// 致命
        };
        /* 1. 拼“时间 [级别] -” 前缀 */
        public string FormatPrefix(LoggingEvent e)
        {
            stringBuilder.Clear();
            return stringBuilder
                .Append(e.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss,fff"))
                .Append(" [")
                .Append(e.Level)
                .Append("] - ")
                .ToString();
        }
        /* 2. 原样返回消息 */
        public string FormatMessage(LoggingEvent e) => e.RenderedMessage;
        /* 3. 颜色函数：背景全部透明，文字查表 */
        public Brush LevelToPrefixBackgroundBrush(Level level) => Brushes.Transparent;
        public Brush LevelToPrefixForegroundBrush(Level level) => Brushes.Black;
        public Brush LevelToTextBackgroundBrush(Level level) => Brushes.Transparent;
        public Brush LevelToTextForegroundBrush(Level level) => LogLevelToTextForegroundBrush.TryGetValue(level, out var b) ? b : Brushes.Black;
    }
}
