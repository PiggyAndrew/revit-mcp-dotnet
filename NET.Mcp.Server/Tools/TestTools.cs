using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModelContextProtocol.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NET.Mcp.Server.Tools
{

    [McpServerToolType]
    public class TestTools
    {
        // Revit Socket服务器配置
        private const string REVIT_SERVER_IP = "127.0.0.1";
        private const int REVIT_SERVER_PORT = 8080;
        private const int SOCKET_TIMEOUT = 30000; // 30秒超时

        /// <summary>
        /// 向Revit发送Socket请求并获取响应
        /// </summary>
        /// <param name="jsonData">要发送的JSON数据</param>
        /// <returns>Revit返回的响应数据</returns>
        private async Task<string> SendToRevitAsync(string jsonData)
        {
            TcpClient client = null;
            try
            {
                // 创建TCP客户端连接
                client = new TcpClient();
                client.ReceiveTimeout = SOCKET_TIMEOUT;
                client.SendTimeout = SOCKET_TIMEOUT;

                // 连接到Revit Socket服务器
                await client.ConnectAsync(REVIT_SERVER_IP, REVIT_SERVER_PORT);
                Console.WriteLine($"已连接到Revit服务器 {REVIT_SERVER_IP}:{REVIT_SERVER_PORT}");

                // 获取网络流
                NetworkStream stream = client.GetStream();

                // 发送数据到Revit
                byte[] data = Encoding.UTF8.GetBytes(jsonData);
                await stream.WriteAsync(data, 0, data.Length);
                Console.WriteLine("数据已发送到Revit");

                // 读取Revit的响应
                byte[] buffer = new byte[4096];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                Console.WriteLine("已接收到Revit响应");
                return response;
            }
            catch (SocketException ex)
            {
                string errorMsg = $"Socket连接错误: {ex.Message}. 请确保Revit Socket服务器正在运行。";
                Console.WriteLine(errorMsg);
                return JsonConvert.SerializeObject(new { error = errorMsg });
            }
            catch (Exception ex)
            {
                string errorMsg = $"发送数据到Revit时出错: {ex.Message}";
                Console.WriteLine(errorMsg);
                return JsonConvert.SerializeObject(new { error = errorMsg });
            }
            finally
            {
                // 确保客户端连接被正确关闭
                client?.Close();
            }
        }
        

        /// <summary>
        /// 创建墙体工具 - 通过Socket通讯向Revit发送创建墙体参数
        /// </summary>
        /// <param name="command">命令名称</param>
        /// <param name="x">起点X坐标</param>
        /// <param name="y">起点Y坐标</param>
        /// <param name="z">起点Z坐标</param>
        /// <param name="x1">终点X坐标</param>
        /// <param name="y2">终点Y坐标</param>
        /// <returns>返回Revit创建的墙体元素数据</returns>
        [McpServerTool(Name = "CreateWall"), Description("Generation Paramaters That Can Create Wall in Revit")]
        public async Task<string> RevitCreateWallTool(string command, double x, double y, double z, double x1, double y2)
        {
            try
            {
                // 构建发送给Revit的命令数据
                var commandData = new
                {
                    command = "CreateWall",
                    args = new
                    {
                        start = new double[] { x, y, z },
                        end = new double[] { x1, y2, z }
                    }
                };

                // 将命令数据序列化为JSON
                string jsonData = JsonConvert.SerializeObject(commandData);
                Console.WriteLine($"发送数据到Revit: {jsonData}");

                // 通过Socket发送数据到Revit并获取响应
                string response = await SendToRevitAsync(jsonData);
                
                Console.WriteLine($"从Revit接收到响应: {response}");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RevitCreateWallTool执行出错: {ex.Message}");
                return JsonConvert.SerializeObject(new { error = ex.Message });
            }
        }
    }
}
