using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSharp_OpenCV_NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CSharp_OpenCV_NET.ViewModels
{
   
    public partial class UserManagerViewModel : ObservableObject
    {
        // 声明一个私有只读字段，用于存储UserModel的引用
        private readonly UserModel _userModel;

        // 通过构造函数注入UserModel
        public UserManagerViewModel(UserModel userModel)
        {
            _userModel = userModel;
        }


        // 暴露UserModel属性供View绑定
        // 这是一个只读属性，返回我们存储的_userModel
        public UserModel User => _userModel;

        #region 保存密码修改命名
        [RelayCommand]
        private void SaveModify(object parameter)
        {
            _userModel.ErrorMessage = "";

            if (parameter == null)
                return;

            var values = (object[])parameter;
            PasswordBox pass1 = values[0] as PasswordBox;
            PasswordBox pass2 = values[1] as PasswordBox;
            if (pass1.Password != pass2.Password)
            {
                _userModel.ErrorMessage = "两次输入密码不匹配";
                return;
            }
            if (string.IsNullOrEmpty(pass1.Password) || pass1.Password.Length < 1)
            {
                _userModel.ErrorMessage = "密码不能为空!";
                return;
            }

            if (!_userModel.ModifyUserPassword(_userModel.SelectedUser, pass1.Password))
            {
                _userModel.ErrorMessage = "密码修改失败,请重试";
                return;
            }

            _userModel.ErrorMessage = $"用户 {_userModel.SelectedUser} 密码修改成功.";
        }
        #endregion
    }
}
