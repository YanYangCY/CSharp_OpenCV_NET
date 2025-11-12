1. 第一部分：该项目为MVVM框架在.NET新版本下的应用.主要借助 [ObservableProperty][RelayCommand]两者特性.
  a. NuGet程序包下载：CommunityToolkit.Mvvm
  b. 新建 Models\ViewModels\Views 三个文件夹
-Model:原始数据、数据库访问、算法
-ViewModel:INotifyPropertyChanged + Command; 使用ObservableCollection< >调用View
-View:XAML+控件+样式
  c. 在Model中新建属性
  d.  在ViewModel中使用[ObservableProperty]来让编译器生成一个"带通知功能"的属性
  e.  在ViewModel中使用[RelayCommand]来实现ICommand接口,将方法转换为命令
  f.  在View中设计界面并绑定数据
  g.  在App.xaml.cs配置依赖注入容器
  h.  新建Services文件夹,用于依赖注入获取窗口
2. 第二部分：导入MaterialDesign3设计规范,方便后期GUI优化
  a. NuGet程序包下载：MaterialDesignThemes、MaterialDesignColors
  b. 全局启用MD3主题：打开App.xaml，添加资源字典加载样式和颜色
3. 第三部分：搭建主窗口菜单栏
  a. 

第二部分：搭建初始化启动页面、主窗口菜单栏、日志模块、GUI优化





第三部分：通讯测试模块


第四部分：搭建相机管理模块

第五部分：图像处理模块

第六部分：细化分支功能：自动定时删图、光源曝光管理、

第七部分：定位模块

第八部分：AI检测算法