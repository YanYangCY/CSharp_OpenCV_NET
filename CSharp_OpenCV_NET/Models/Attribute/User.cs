using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_OpenCV_NET.Models.Attribute
{

    public partial class User : ObservableObject
    {
        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private int _level;


        // 无参构造函数 - JSON 反序列化需要
        public User()
        { 
        }
        public User(string name, string password, int level)
        {
            _name = name;
            _password = password;
            _level = level;
        }
    }
}
