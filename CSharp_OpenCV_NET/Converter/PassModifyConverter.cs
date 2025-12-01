using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CSharp_OpenCV_NET.Converter
{
    /// <summary>
    /// 密码修改多值转换器
    /// 这个转换器实现了 IMultiValueConverter 接口，专门用于处理 WPF 多值绑定场景。
    /// </summary>
    public class PassModifyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // 参数验证：确保输入值不为空且包含预期数量的元素
            if (values == null || values.Length != 2)
                return null;
            // 返回值的克隆，避免原始数据被意外修改
            return values.Clone();
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // 明确抛出异常，表明此转换器不支持反向转换
            throw new NotImplementedException();
        }
    }
}
