using Microsoft.Extensions.Hosting;
using SuperSocket.Server.Abstractions;
using SuperSocket.Server.Abstractions.Session;
using SuperSocket.Server.Host;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_OpenCV_NET.Communicate.TCP
{
    /* Socket 服务器主类
     * 
     * 架构说明：
     * 本类使用 SuperSocket 框架作为底层通信引擎，SuperSocket 是一个高性能的 .NET Socket 服务器框架。
     * 通过依赖注入和配置方式构建服务器，提供异步、事件驱动的网络编程模型。
     * 
     * 职责：
     * 1. 管理 SuperSocket 服务器的生命周期（启动、停止）
     * 2. 配置服务器参数（端口、监听地址等）
     * 3. 处理客户端连接和断开事件
     * 4. 接收和处理客户端发送的数据包
     * 5. 向客户端发送响应数据
     * 
     * 工作流程：
     * 1. 调用 Start() 方法启动服务器
     * 2. 监听指定端口，等待客户端连接
     * 3. 客户端连接时触发连接事件
     * 4. 客户端发送数据时，经过 SimplePipelineFilter 解析为完整消息
     * 5. 调用 UsePackageHandler 处理消息
     * 6. 客户端断开时触发断开事件
     * 7. 调用 Stop() 方法停止服务器
     */
    public class SocketServer
    {
        /// <summary>
        /// SuperSocket 主机实例
        /// 
        /// 这是 SuperSocket 服务器的核心，管理所有网络连接和会话。
        /// IHost 是 .NET 通用的主机接口，SuperSocket 基于此构建。
        /// 
        /// 为什么使用 IHost：
        /// 1. 提供统一的生命周期管理
        /// 2. 支持依赖注入
        /// 3. 集成配置系统和日志系统
        /// 4. 支持优雅关闭和重启
        /// 
        /// 注意：使用可空类型，因为服务器可能未启动或已停止。
        /// </summary>
        private IHost? _host;

        // ★★★ 添加：保存所有连接的会话 ★★★
        private readonly ConcurrentDictionary<string, IAppSession> _sessions = new();
        public bool IsRunning => _host != null;


        /// <summary>
        /// 启动 Socket 服务器
        /// 
        /// 参数：
        /// int port - 监听的端口号，默认 60001
        /// 
        /// 返回：
        /// Task - 异步任务，表示服务器启动过程
        /// 
        /// 执行流程：
        /// 1. 记录启动日志
        /// 2. 构建 SuperSocket 主机
        /// 3. 配置服务器选项
        /// 4. 注册事件处理器
        /// 5. 启动服务器
        /// 
        /// 注意事项：
        /// 1. 此方法会阻塞直到服务器完全启动
        /// 2. 启动失败会抛出异常
        /// 3. 端口被占用会导致启动失败
        /// 4. 需要有管理员权限才能监听某些端口（如 80、443）
        /// </summary>
        public async Task Start(int port = 60001)
        {
            // 记录服务器启动日志，便于调试和监控
            Debug.WriteLine($"正在启动Socket服务器，端口: {port}");
            // 使用 .NET 的通用主机构建器创建 SuperSocket 服务器
            // Host.CreateDefaultBuilder() 提供了默认的配置、日志和依赖注入容器
            _host = Host.CreateDefaultBuilder()
                // 将通用主机转换为 SuperSocket 主机
                // BinaryMessage: 数据包模型，表示一个完整的消息
                // SimplePipelineFilter: 协议过滤器，用于解析网络字节流为数据包
                .AsSuperSocketHostBuilder<BinaryMessage, SimplePipelineFilter>()
                // 配置 SuperSocket 服务器选项
                // 包括监听地址、端口、缓冲区大小、超时设置等
                .ConfigureSuperSocket(options =>
                {
                    // 配置监听器列表，可以同时监听多个端口
                    options.Listeners = new List<ListenOptions>
                    {
                        // 创建一个监听器
                        // Ip = "0.0.0.0" 表示监听所有网络接口（IPv4）
                        // 可以使用 "127.0.0.1" 只监听本机
                        new ListenOptions { Ip = "0.0.0.0", Port = port , BackLog = 1000 }
                        // 可以添加多个监听器，如同时监听 IPv6
                        // new ListenOptions { Ip = "::", Port = port }  // IPv6
                        
                    };
                    // 可选的配置项：
                    // options.MaxConnectionNumber = 1000;  // 最大连接数
                    // options.IdleSessionTimeout = 300;    // 空闲会话超时（秒）
                    // options.ReceiveBufferSize = 8192;    // 接收缓冲区大小
                    // options.SendBufferSize = 8192;       // 发送缓冲区大小
                    // options.Logging.CommandLogLevel = LogLevel.Debug;  // 日志级别
                })
                // ★★★ 添加会话处理器 ★★★
                // 注册会话处理器 - 处理客户端连接和断开事件
                // 第一个委托：客户端连接时触发
                // 第二个委托：客户端断开时触发，包含断开原因
                .UseSessionHandler(
                // 客户端连接成功时执行
                async (session) =>
                {
                    // ★★★ 保存会话到字典 ★★★
                    _sessions.TryAdd(session.SessionID, session);
                    // 记录连接信息
                    Debug.WriteLine($"客户端连接成功: {session.SessionID}");
                    Debug.WriteLine($"远程地址: {session.RemoteEndPoint}");

                    // 可选：向客户端发送欢迎消息
                    // 这有助于确认连接成功，并告知客户端服务器已就绪
                    string welcome = "Welcome to Server!\n";
                    // 将字符串转换为 UTF-8 编码的字节数组
                    // 注意：字符串末尾要加上换行符 \n，以便客户端知道消息结束
                    await session.SendAsync(Encoding.UTF8.GetBytes(welcome));
                    Debug.WriteLine($"已发送欢迎消息: {welcome.Trim()}");
                },
                // 客户端断开连接时执行
                async (session, reason) =>
                {
                    // ★★★ 从字典中移除会话 ★★★
                    _sessions.TryRemove(session.SessionID, out _);

                    Debug.WriteLine($"客户端断开: {session.SessionID}, 原因: {reason}");
                })
                // 注册数据包处理器 - 处理客户端发送的数据
                // 当 SimplePipelineFilter 解析出一个完整的数据包后，会调用此处理器
                .UsePackageHandler(async (session, package) =>
                {
                    // ★★★ 这里添加一个醒目的标识 ★★★
                    Debug.WriteLine("★★★ 进入 UsePackageHandler ★★★");
                    // 处理收到的数据包
                    // 这里写你的业务逻辑
                    try
                    {
                        // 显示接收信息
                        Debug.WriteLine($"=== 收到消息 ===");
                        Debug.WriteLine($"客户端: {session.SessionID}");
                        //Debug.WriteLine($"时间: {package.ReceiveTime:HH:mm:ss.fff}");
                        Debug.WriteLine($"数据长度: {package.RawData.Length} 字节");
                        // 将字节数组转换为十六进制字符串，便于查看原始数据
                        string hexString = BitConverter.ToString(package.RawData);
                        Debug.WriteLine($"十六进制: {hexString}");

                        // 自动回复（可选）
                        // 根据业务需求处理数据，这里只是简单回复
                        // 实际应用中可能涉及：
                        // 1. 图像处理
                        // 2. 数据分析
                        // 3. 数据库操作
                        // 4. 调用其他服务
                        await SendResponse(session, package.RawData);
                    }
                    catch
                    {
                        Debug.WriteLine($"收到来自 {session.SessionID} 的二进制数据，长度: {package.RawData.Length} 字节");
                    }
                })
                // 构建 SuperSocket 主机实例
                // 这一步会验证配置并创建所有必要的组件
                .Build();
            // 启动 SuperSocket 服务器
            // 开始监听指定端口，接受客户端连接
            await _host.StartAsync();
            // 启动后也输出提示
            Debug.WriteLine($"Socket 服务器已启动，监听端口: {port}");
        }

        /// <summary>
        /// 回复接收到的数据ECHO
        /// </summary>
        /// <param name="session"></param>
        /// <param name="receivedData"></param>
        /// <returns></returns>
        private async Task SendResponse(IAppSession session, byte[] receivedData)
        {
            try
            {
                string responseText = Encoding.UTF8.GetString(receivedData);
                byte[] responseBytes = Encoding.UTF8.GetBytes(responseText);
                await session.SendAsync(responseBytes);
                Debug.WriteLine($"已发送回复: {responseText.Trim()}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"发送回复失败: {ex.Message}");
            }
        }


        // ★★★ 新增：发送给所有连接的客户端 ★★★
        public async Task SendToAllAsync(byte[] data)
        {
            if (!IsRunning)
            {
                Debug.WriteLine("错误：服务器未运行，无法发送消息");
                return;
            }

            if (_sessions.IsEmpty)
            {
                Debug.WriteLine("提示：当前没有连接的客户端");
                return;
            }

            var tasks = new List<Task>();
            foreach (var session in _sessions.Values)
            {
                try
                {
                    tasks.Add(session.SendAsync(data).AsTask());
                    Debug.WriteLine($"正在向客户端 {session.SessionID} 发送消息");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"向客户端 {session.SessionID} 发送失败: {ex.Message}");
                }
            }

            await Task.WhenAll(tasks);
            Debug.WriteLine($"已向 {_sessions.Count} 个客户端发送消息");
        }

        // ★★★ 新增：发送给特定客户端 ★★★
        public async Task SendToClientAsync(string sessionId, byte[] data)
        {
            if (!IsRunning)
            {
                Debug.WriteLine("错误：服务器未运行，无法发送消息");
                return;
            }

            if (_sessions.TryGetValue(sessionId, out var session))
            {
                try
                {
                    await session.SendAsync(data);
                    Debug.WriteLine($"已发送消息到客户端: {sessionId}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"向客户端 {sessionId} 发送失败: {ex.Message}");
                }
            }
            else
            {
                Debug.WriteLine($"错误：客户端 {sessionId} 不存在或已断开连接");
            }
        }

        // ★★★ 新增：获取所有连接的客户端 ★★★
        public List<string> GetConnectedClients()
        {
            return _sessions.Keys.ToList();
        }

        // ★★★ 新增：获取连接客户端数量 ★★★
        public int GetConnectedClientCount()
        {
            return _sessions.Count;
        }


        /// <summary>
        /// 停止Socket服务器
        /// </summary>
        /// <returns></returns>
        public async Task Stop()
        {
            await _host.StopAsync();
        }
    }
}
