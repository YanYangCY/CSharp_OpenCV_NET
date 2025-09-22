第一部分：该项目为MVVM框架在.NET新版本下的应用.主要借助 [ObservableProperty][RelayCommand]两者特性.
1. NuGet程序包下载：CommunityToolkit.Mvvm
2. 新建 Models\ViewModels\Views 三个文件夹
* -Model:原始数据、数据库访问、算法
* -ViewModel:INotifyPropertyChanged + Command; 使用ObservableCollection< >调用View
* -View:XAML+控件+样式
3. 在Model中新建属性
4. 在ViewModel中使用[ObservableProperty]来让编译器生成一个"带通知功能"的属性
5. 在ViewModel中使用[RelayCommand]来实现ICommand接口,将方法转换为命令
6. 在View中设计界面并绑定数据
7. 在App.xaml.cs配置依赖注入容器
8. 新建Services文件夹,用于依赖注入获取窗口