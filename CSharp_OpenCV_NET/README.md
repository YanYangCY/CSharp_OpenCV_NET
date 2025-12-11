1. 第一部分：该项目为MVVM框架在.NET新版本下的应用.主要借助 [ObservableProperty][RelayCommand]两者特性.
  a. NuGet程序包下载：CommunityToolkit.Mvvm
  b. 新建 Models\ViewModels\Views 三个文件夹
-Model:原始数据、数据库访问、算法
-ViewModel:INotifyPropertyChanged + Command; 使用ObservableCollection< >调用View
-View:XAML+控件+样式
  c. 在Model中新建属性
  d.  在ViewModel中使用[ObservableProperty]来让编译器生成一个"带通知功能"的属性
  e.  在ViewModel中使用[RelayCommand]来实现ICommand接口，将方法转换为命令
  f.  在View中设计界面并绑定数据
  g.  在App.xaml.cs配置依赖注入容器
  h.  新建Services文件夹，用于依赖注入获取窗口（未实际使用）
2. 第二部分：导入MaterialDesign3设计规范,方便后期GUI优化
  a. NuGet程序包下载：MaterialDesignThemes、MaterialDesignColors
  b. 全局启用MD3主题：打开App.xaml，添加资源字典加载样式和颜色
3. 第三部分：搭建主窗口菜单栏
  a. 新建用户控件，调整布局
  b. 在主页面中引入菜单栏控件
4. 第四部分：搭建初始化启动页面
  a. 在Views文件夹中新建SplashWindow页面，用于启动显示
  b. 新建Assets文件夹用来存放管理核心资源目录：字体、图片、图标等
  c. 窗体加载就执行命令：NuGet程序包下载：Microsoft.Xaml.Behaviors.Wpf
  d. 在SplashViewModel中新建[RelayCommand]，用来初始化一些加载操作
  e. 在App.xaml.cs中注册vm、窗体到容器
5. 第五部分：新建全局存储路径、引入日志模块
  a. 新建Configuration文件夹集中管理路径、配置项
  b. 新建AppPath.cs用于配置静态路径，（同时默认放置在初始化启动执行新建文件夹）
  c. NuGet程序包下载：log4net（这边使用的原生log4net，未使用Log4NetWrapperLite这种非官方封装）
  d. 新建Log文件夹存放：log4net.config、 CustomLogFormatter.cs、MTLogger.cs、ILogFormatter.cs （固定套路）
  e. 关于log相关存放位置、命名等修改都在log4net.config中进行配置
6. 第六部分：Model的注册和调用（这部分比较复杂，需要多熟练使用）
  a. 给模型添加可通知属性；在App.xaml.cs里面依赖注册Model的单例
  b. 可以在VM里面声明只读属性来调用Model，通过构造函数注入由DI容器自动提供的参数，暴露Model的数据供View绑定
  c. 在第七部分建立用户管理窗口时发现窗口绑定时需要创建一个专门的窗口服务来管理窗口的创建和显示
  d. 改写NavigationService，这样调用窗口的时候直接调用INavigationService中方法名称（目前的方法是单个View和ViewModel进行绑定）
7. 第七部分：用户登录权限分级
  a. 菜单栏新建用户登录按钮
  b. 新建UserLoginWindow登录窗口并注册到容器
  c. 图片资源:复制到输出目录(始终复制)-生成操作(内容) *这里不设置会找不到资源*
  d. 在菜单栏对应按钮功能方法下取出窗口并显示
  e. 界面布局，Models文件夹下新建分级文件夹Attribute属性文件夹用于存放属性
  f. 更改技术路线为JSON序列化,NuGet程序包下载：Newtonsoft.Json
  g. 新建User.cs存放可序列化的User类,UserModel中创建默认用户JSON文件创建,并在初始化中进行调用
  h. 新建对应的用户管理窗口，可进行修改密码
  i. 登录高权限账号后，长时间无操作自动降级
8. 第八部分：通讯模块-TCP、ADS   
  a. NuGet程序包下载：SuperSocket、SuperSocket.ProtoBase
  b. 创建TCP文件夹,创建相关文件
  c. 将SocketServer在App.xaml.cs中注册单例,在SplashViewModel中进行初始化




第三部分：通讯测试模块、配方模块


第四部分：搭建相机管理模块

第五部分：图像处理模块

第六部分：细化分支功能：自动定时删图、光源曝光管理、

第七部分：定位模块

第八部分：AI检测算法