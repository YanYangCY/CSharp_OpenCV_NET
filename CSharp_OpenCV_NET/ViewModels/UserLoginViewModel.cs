using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CSharp_OpenCV_NET.Models;
using CSharp_OpenCV_NET.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CSharp_OpenCV_NET.ViewModels
{
    public partial class UserLoginViewModel : ObservableObject
    {
        // 声明一个私有只读字段，用于存储UserModel的引用
        private readonly UserModel _userModel;
        private readonly INavigationService _navigationService;

        // 通过构造函数注入UserModel
        public UserLoginViewModel(UserModel userModel, INavigationService navigationService)
        {
            _userModel = userModel;
            _navigationService = navigationService;
        }

        // 暴露UserModel属性供View绑定
        // 这是一个只读属性，返回我们存储的_userModel
        public UserModel User => _userModel;


        #region 登录按键功能
        [RelayCommand]
        private void Validate(PasswordBox obj)
        {
            _userModel.ErrorMessage = "";
            string password = (obj == null ? "" : obj.Password);
            if (!_userModel.Validation(_userModel.SelectedUser, password))
            {
                //_userModel.ErrorMessage = "密码不正确!";
                return;
            }
            _userModel.ErrorMessage = "密码正确!";
            // UI线程异步执行
            App.Current.Dispatcher.BeginInvoke(() =>
            {
                // 消息内容+令牌
                WeakReferenceMessenger.Default.Send<string, string>("close", "CloseLoginWindow");
            });

        }
        #endregion

        #region 用户管理按键功能
        [RelayCommand(CanExecute = nameof(CanAccessUserManager))]
        private void UserManager()
        {
            _navigationService.ShowUserManagerDialog();
        }
        private bool CanAccessUserManager()
        {
            return _userModel.CurrentUser?.Level == 3;
        }
        #endregion


    }
}
