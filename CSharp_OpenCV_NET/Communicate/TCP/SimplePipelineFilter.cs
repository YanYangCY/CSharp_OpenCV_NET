using SuperSocket.ProtoBase;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_OpenCV_NET.Communicate.TCP
{
    /* 主要流程：客户端发送 → 网络缓冲区 → Filter() → 基类查找分隔符 → DecodePackage() → UsePackageHandler()
     * 协议过滤器 - 使用换行符(\n)作为消息分隔符
     * 继承自 TerminatorPipelineFilter<BinaryMessage>，这是一个基于分隔符的过滤器
     * 当接收到数据时，它会查找指定的分隔符，找到分隔符后，将分隔符之前的数据作为一个完整的消息
     * 使用场景：客户端发送的数据以换行符结束，如 "Hello\n"、"OK\n" 等
     * 
     * 工作原理：
     * 1. 客户端发送数据，例如 "Hello\n"
     * 2. 过滤器等待直到遇到分隔符 \n (0x0A)
     * 3. 找到分隔符后，提取分隔符之前的数据 "Hello"
     * 4. 调用 DecodePackage 方法将数据封装为 BinaryMessage
     * 5. 将 BinaryMessage 传递给 UsePackageHandler 处理
     * 
     * 注意：如果客户端发送的数据不带分隔符，过滤器会一直等待，直到超时或连接关闭
     */
    public class SimplePipelineFilter : TerminatorPipelineFilter<BinaryMessage>
    {
        // 构造函数 - 初始化过滤器，使用换行符(\n)作为分隔符
        public SimplePipelineFilter() : base(new byte[] { (byte)'\n' })
        {
            // 创建过滤器实例时的日志，用于调试
            Debug.WriteLine("使用换行符分隔符过滤器");
        }
        /// <summary>
        /// 解码数据包 - 将原始字节数据转换为业务层可用的消息对象
        /// 
        /// 触发时机：当过滤器收集到完整的数据包（即找到分隔符）时调用
        /// 
        /// 参数：
        /// ref ReadOnlySequence<byte> buffer - 只读字节序列，包含分隔符之前的所有数据
        ///                             注意：SuperSocket 已经自动移除了末尾的分隔符
        /// 
        /// 返回值：
        /// BinaryMessage - 封装好的消息对象，包含原始字节数据和可能的元数据
        /// 
        /// 处理流程：
        /// 1. 将字节序列转换为字节数组
        /// 2. 记录调试信息（长度、内容等）
        /// 3. 创建 BinaryMessage 对象
        /// 4. 返回消息对象供后续处理
        /// </summary>
        protected override BinaryMessage DecodePackage(ref ReadOnlySequence<byte> buffer)
        {
            try
            {
                // buffer 已经去掉了最后的换行符 \n
                var data = buffer.ToArray();

                // 调试信息：记录收到的消息长度和原始字节
                Debug.WriteLine($"收到消息，长度: {data.Length} 字节");
                Debug.WriteLine($"原始字节: {BitConverter.ToString(data)}");

                // 如果是文本数据，尝试显示
                if (IsTextData(data))
                {
                    string text = Encoding.UTF8.GetString(data);
                    Debug.WriteLine($"文本内容: '{text}'");
                }
                // 创建并返回消息对象
                // 注意：这里可以根据需要添加更多属性，如接收时间、发送方信息等
                return new BinaryMessage
                {
                    RawData = data, // 原始二进制数据
                    //ReceiveTime = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"解码数据包异常: {ex.Message}");
                return null;   // 返回 null 表示处理失败，SuperSocket 会丢弃这个数据包
            }
        }
        /// <summary>
        /// 判断是否为文本数据 - 辅助方法
        /// 
        /// 参数：
        /// byte[] data - 要判断的字节数组
        /// 
        /// 返回值：
        /// bool - true表示数据可能是文本，false表示可能是二进制数据
        /// 
        /// 判断逻辑：
        /// 遍历所有字节，检查是否包含不可打印的控制字符（ASCII < 32）
        /// 但允许常见的空白字符：制表符(9)、换行符(10)、回车符(13)
        /// 
        /// 注意：这是一个简单的启发式判断，不适用于所有情况
        /// 例如：UTF-8 编码的中文字符不会触发此判断
        /// </summary>
        private bool IsTextData(byte[] data)
        {
            // 简单判断是否为文本数据
            foreach (byte b in data)
            {
                // 控制字符（除了常见的空白符）
                // ASCII 0-31 是控制字符，但以下字符通常出现在文本中：
                // 9  = \t (制表符)
                // 10 = \n (换行符)  
                // 13 = \r (回车符)
                if (b < 32 && b != 9 && b != 10 && b != 13)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 重置过滤器状态
        /// 
        /// 触发时机：当过滤器处理完一个完整消息后，准备处理下一个消息时
        /// 
        /// 作用：
        /// 1. 重置内部状态，准备接收新的消息
        /// 2. 清理临时缓冲区
        /// 3. 确保过滤器可以连续处理多个消息
        /// 
        /// 注意：基类的 Reset() 方法会重置过滤器的基础状态
        /// 如果自定义了状态变量，需要在这里重置
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            Debug.WriteLine("过滤器被重置");
        }

    }
}
