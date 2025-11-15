using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CSharp_OpenCV_NET.Log
{
    /// <summary>
    /// 负责把“日志级别”翻译成“颜色 + 文本”的调色板
    /// 任何想彩色输出日志的控件（RichTextBox/ListView）都必须实现它
    /// </summary>
    public interface ILogFormatter
    {
        /* 1. 文本部分 */
        string FormatPrefix(LoggingEvent loggingEvent);
        string FormatMessage(LoggingEvent loggingEvent);

        /* 2. 颜色部分 */
        Brush LevelToPrefixBackgroundBrush(Level level);
        Brush LevelToPrefixForegroundBrush(Level level);
        Brush LevelToTextBackgroundBrush(Level level);
        Brush LevelToTextForegroundBrush(Level level);
    }
}
