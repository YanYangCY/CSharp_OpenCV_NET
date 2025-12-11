using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_OpenCV_NET.Communicate.TCP
{
    /// <summary>
    /// 数据包模型
    /// </summary>
    public class BinaryMessage
    {
        public byte[] RawData { get; set; }  // 原始二进制数据
    }
}
