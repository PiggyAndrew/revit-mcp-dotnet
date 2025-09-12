using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RevitTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Tuna.Revit.Extension;

namespace RevitTest.Commands
{
    /// <summary>
    /// 创建墙体的外部事件处理器
    /// </summary>
    public class CreateWallEventHandler : IExternalEventHandler
    {
        /// <summary>
        /// 墙体数据
        /// </summary>
        public CreateWallArguments WallData { get; set; }
        
        /// <summary>
        /// 执行结果
        /// </summary>
        public string Result { get; set; }
        
        /// <summary>
        /// 执行创建墙体操作
        /// </summary>
        /// <param name="app">Revit应用程序</param>
        public void Execute(UIApplication app)
        {
            try
            {
                var document = app.ActiveUIDocument.Document;
                
                if (WallData?.Start == null || WallData?.End == null)
                {
                    Result = JsonConvert.SerializeObject(new { error = "参数格式错误：缺少start或end坐标" });
                    return;
                }

                Element element = null;
                
                // 创建起点和终点
                var startPoint = new XYZ(WallData.Start[0], WallData.Start[1], WallData.Start[2]);
                var endPoint = new XYZ(WallData.End[0], WallData.End[1], WallData.End[2]);
                
                // 检查起点和终点是否相同
                if (startPoint.IsAlmostEqualTo(endPoint))
                {
                    Result = JsonConvert.SerializeObject(new { error = "起点和终点不能相同" });
                    return;
                }

                // 创建线段
                var curve = Line.CreateBound(startPoint, endPoint);
                IList<Curve> curves = new List<Curve> { curve };

                // 在事务中创建墙体
                using (Transaction trans = new Transaction(document, "创建墙体"))
                {
                    trans.Start();
                    try
                    {
                        element = Wall.Create(document,curve, document.ActiveView.GenLevel.Id,false);
                        trans.Commit();
                        
                        // 设置成功结果
                        Result = JsonConvert.SerializeObject(new 
                        { 
                            success = true, 
                            elementId = element.Id.IntegerValue,
                            message = "墙体创建成功"
                        });
                    }
                    catch (Exception ex)
                    {
                        trans.RollBack();
                        Result = JsonConvert.SerializeObject(new { error = $"创建墙体失败: {ex.Message}" });
                    }
                }
            }
            catch (Exception ex)
            {
                Result = JsonConvert.SerializeObject(new { error = $"处理命令时出错: {ex.Message}" });
            }
        }

        /// <summary>
        /// 获取事件处理器名称
        /// </summary>
        /// <returns>事件处理器名称</returns>
        public string GetName()
        {
            return "CreateWallEventHandler";
        }
    }

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class RevitSocketServerCommand : IExternalCommand
    {
        private static TcpListener _server;
        private static bool _isRunning = false;
        private static Thread _serverThread;
        private Document _document;
        private static ExternalEvent _externalEvent;
        private static CreateWallEventHandler _eventHandler;

        public void StartSocketServer()
        {
            try
            {
                _server = new TcpListener(IPAddress.Parse("127.0.0.1"), 8080);
                _server.Start();

                while (_isRunning)
                {
                    try
                    {
                        // 等待客户端连接
                        TcpClient client = _server.AcceptTcpClient();

                        // 处理请求
                        Task.Run(() => HandleClientRequest(client));

                    }
                    catch (Exception ex)
                    {
                        // 记录异常但继续运行
                        System.Diagnostics.Debug.WriteLine($"Socket服务异常：{ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Socket服务严重错误：{ex.Message}");
                _isRunning = false;
            }
            finally
            {
                if (_server != null)
                {
                    _server.Stop();
                }
            }
        }

        private async void HandleClientRequest(TcpClient client)
        {
            using (client)
            {
                try
                {
                    // 获取网络流
                    NetworkStream stream = client.GetStream();

                    // 读取客户端发送的数据
                    byte[] buffer = new byte[4096];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string requestString = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    // 解析请求
                    JObject request = JObject.Parse(requestString);
                    string command = request["command"].ToString();
                    JObject args = (JObject)request["args"];

                    // 处理命令并获取响应
                    object response =  ProcessCommandAsync(args);

                    // 将响应转换为JSON并发送回客户端
                    string responseString = JsonConvert.SerializeObject(response);
                    byte[] responseData = Encoding.UTF8.GetBytes(responseString);
                    stream.Write(responseData, 0, responseData.Length);
                }
                catch (Exception ex)
                {
                    // 处理异常
                    string errorResponse = JsonConvert.SerializeObject(new { error = ex.Message });
                    byte[] errorData = Encoding.UTF8.GetBytes(errorResponse);
                    client.GetStream().Write(errorData, 0, errorData.Length);
                }
            }
        }

        /// <summary>
        /// 处理来自MCP服务器的命令请求
        /// </summary>
        /// <param name="args">命令参数</param>
        /// <returns>执行结果</returns>
        private string ProcessCommandAsync(JObject? args)
        {
            try
            {
                // MessageBox.Show($"接收到参数: {JsonConvert.SerializeObject(args)}");

                // 解析参数为CreateWallArguments对象
                var data = args.ToObject<CreateWallArguments>();
                if (data?.Start == null || data?.End == null)
                {
                    return JsonConvert.SerializeObject(new { error = "参数格式错误：缺少start或end坐标" });
                }

                // 设置事件处理器的数据
                _eventHandler.WallData = data;
                
                // 触发外部事件
                _externalEvent.Raise();
                
                // 等待事件处理完成（简单的轮询方式）
                int timeout = 10000; // 10秒超时
                int elapsed = 0;
                int interval = 100; // 100毫秒检查间隔
                
                while (string.IsNullOrEmpty(_eventHandler.Result) && elapsed < timeout)
                {
                    Thread.Sleep(interval);
                    elapsed += interval;
                }
                
                if (elapsed >= timeout)
                {
                    return JsonConvert.SerializeObject(new { error = "操作超时" });
                }
                
                string result = _eventHandler.Result;
                _eventHandler.Result = null; // 清空结果以备下次使用
                
                return result;
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { error = $"处理命令时出错: {ex.Message}" });
            }
        }




        // 提供一个停止服务的方法
        public static void StopServer()
        {
            if (_isRunning)
            {
                _isRunning = false;
                if (_server != null)
                {
                    _server.Stop();
                }
            }
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                _document = commandData.Application.ActiveUIDocument.Document;
                
                // 初始化外部事件处理器
                if (_eventHandler == null)
                {
                    _eventHandler = new CreateWallEventHandler();
                    _externalEvent = ExternalEvent.Create(_eventHandler);
                }

                if (!_isRunning)
                {
                    // 启动服务器
                    _isRunning = true;
                    _serverThread = new Thread(StartSocketServer);
                    _serverThread.IsBackground = true;
                    _serverThread.Start();

                    TaskDialog.Show("Revit Socket服务", "Revit Socket服务已启动，监听端口：8080");
                    return Result.Succeeded;
                }
                else
                {
                    TaskDialog.Show("Revit Socket服务", "服务已在运行中");
                    return Result.Succeeded;
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("错误", $"启动Socket服务失败：{ex.Message}");
                return Result.Failed;
            }
        }
    }
}