using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSharp_OpenCV_NET.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_OpenCV_NET.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        // 声明一个私有只读字段，用于存储AppStatusModel的引用
        private readonly AppStatusModel _appStatus;

        // 通过构造函数注入AppStatusModel
        // 这个appStatus参数是由DI容器自动提供的
        public MainViewModel(AppStatusModel appStatus)
        {
            // 将传入的appStatus赋值给私有字段_appStatus
            _appStatus = appStatus;
        }

        // 暴露AppStatus属性供View绑定
        // 这是一个只读属性，返回我们存储的_appStatus
        public AppStatusModel AppStatus => _appStatus;

        [RelayCommand]
        private void SwitchRunMode()
        {
            // 切换运行状态
            //_appStatus.IsOnline = !_appStatus.IsOnline;
            _appStatus.NumAdd++;
            //bool sb = _appStatus.IsOnline;
        }

        [RelayCommand]
        private void ButtonAddNum()
        {
            _appStatus.NumAdd++;
        }
    }
}
