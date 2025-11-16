using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_OpenCV_NET.Models
{
    public partial class AppStatusModel : ObservableObject
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppStatusModel() 
        {
            IsOnline = false;
            InitStatus = 0xFE;
            NumAdd = 0;
        }
        #region 可通知属性
        // 运行状态
        [ObservableProperty]
        private bool _isOnline;

        [ObservableProperty]
        private int _initStatus;

        [ObservableProperty]
        private int _numAdd;
        #endregion
    }
}
