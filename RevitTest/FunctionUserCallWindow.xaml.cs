using Autodesk.Revit.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using Autodesk.Revit.DB;
using static RevitTest.Command;
using static Autodesk.Revit.DB.SpecTypeId;
using ModelContextProtocol.Client;
using OpenAI;
using OpenAI.Chat;
using Microsoft.Extensions.AI;
using System.ClientModel;
using Revit.Async;
using System.Net.Http;
using RevitTest.Models;


namespace RevitTest
{
    public class CreateWallData
    {
        [JsonProperty(PropertyName = "command")]
        public string Command { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "arguments")]
        public CreateWallArguments Args { get; set; }
    }

    public class CreateWallArguments
    {
        [JsonProperty(PropertyName = "start")]
        public double[] Start { get; set; }
        [JsonProperty(PropertyName = "end")]
        public double[] End { get; set; }
    }

    public class ContentItem
    {
        public string Type { get; set; }
        public string Text { get; set; }
    }

    public class ResponseData
    {
        public List<ContentItem> Content { get; set; }
        public bool IsError { get; set; }
    }
    
    // ChatResponse类定义，用于接收WebAPI返回的数据
    public class ChatResponse
    {
        [JsonProperty("messages")]
        public List<ChatResponseMessage> Messages { get; set; } = new List<ChatResponseMessage>();
        
        [JsonProperty("text")]
        public string Text { get; set; }
    }
    
    public class ChatResponseMessage
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        
        [JsonProperty("role")]
        public string Role { get; set; }
        
        [JsonProperty("contents")]
        public List<ChatResponseContent> Contents { get; set; } = new List<ChatResponseContent>();
    }
    
    public class ChatResponseContent
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    /// <summary>
    /// FunctionUserCallWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FunctionUserCallWindow : Window
    {
        private string userInput;
        private readonly UIDocument uiDocument;
        private Document document;

        public FunctionUserCallWindow(ExternalCommandData commandData)
        {
            InitializeComponent();
            uiDocument = commandData.Application.ActiveUIDocument;
            document = uiDocument.Document;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
             userInput = this.TextBox.Text.Trim();
            if (!string.IsNullOrEmpty(userInput))
            {
                // 添加用户消息
                AddUserMessage(userInput);
                
                // 清空输入框
                this.TextBox.Text = string.Empty;
                
                // 显示思考动画
                ThinkingIndicator.Visibility = System.Windows. Visibility.Visible;
                StartThinkingAnimation();
                
                // 发送消息
                await Chat();
            }
        }
        
        // 添加用户消息
        private void AddUserMessage(string message)
        {
            Border messageBorder = new Border
            {
                Style = (Style)FindResource("UserMessageStyle")
            };
            
            TextBlock messageText = new TextBlock
            {
                Text = message,
                Style = (Style)FindResource("MessageTextStyle")
            };
            
            messageBorder.Child = messageText;
            MessagePanel.Children.Add(messageBorder);
            
            // 滚动到底部
            MessageScrollViewer.ScrollToEnd();
        }
        
        // 添加AI消息
        private void AddAIMessage(string message)
        {
            Border messageBorder = new Border
            {
                Style = (Style)FindResource("AIMessageStyle")
            };
            
            TextBlock messageText = new TextBlock
            {
                Text = message,
                Style = (Style)FindResource("MessageTextStyle")
            };
            
            messageBorder.Child = messageText;
            MessagePanel.Children.Add(messageBorder);
            
            // 滚动到底部
            MessageScrollViewer.ScrollToEnd();
        }
        
        // 启动思考动画
        private void StartThinkingAnimation()
        {
            // 创建动画
            DoubleAnimation opacityAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.3,
                Duration = TimeSpan.FromSeconds(0.7),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };
            
            // 应用动画到点
            Dot1.BeginAnimation(OpacityProperty, opacityAnimation);
            
            // 为其他点创建延迟动画
            DoubleAnimation opacityAnimation2 = opacityAnimation.Clone();
            opacityAnimation2.BeginTime = TimeSpan.FromSeconds(0.2);
            Dot2.BeginAnimation(OpacityProperty, opacityAnimation2);
            
            DoubleAnimation opacityAnimation3 = opacityAnimation.Clone();
            opacityAnimation3.BeginTime = TimeSpan.FromSeconds(0.4);
            Dot3.BeginAnimation(OpacityProperty, opacityAnimation3);
        }
        
        // 停止思考动画
        private void StopThinkingAnimation()
        {
            Dot1.BeginAnimation(OpacityProperty, null);
            Dot2.BeginAnimation(OpacityProperty, null);
            Dot3.BeginAnimation(OpacityProperty, null);
            
            Dot1.Opacity = 1.0;
            Dot2.Opacity = 1.0;
            Dot3.Opacity = 1.0;
        }


        private async Task Chat()
        {
            try
            {
                // 配置HttpClientHandler以处理自签名证书
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                };
                
                // 创建HttpClient实例
                using (var httpClient = new HttpClient(handler))
                {
                    // 设置API基础地址
                    string baseUrl = "http://localhost:5024"; // 使用HTTPS协议和正确的SSL端口
                    httpClient.BaseAddress = new Uri(baseUrl);
                    
                    // 添加跨域请求头
                    httpClient.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");

                    // 构建请求URL，传递用户输入
                    string requestUrl = $"/RevitTest?userInput={userInput}";

                    // 发送GET请求到RevitTestController
                    var response = await httpClient.GetAsync(requestUrl);
                    
                    // 确保请求成功
                    response.EnsureSuccessStatusCode();

                    // 读取响应内容
                    var chatResponse = await response.Content.ReadAsStringAsync();

                    // 显示响应结果
                    if (chatResponse != null)
                    {
                        // 将ChatResponse转换为可显示的文本
                        var responseText = JsonConvert.DeserializeObject<ChatResponse>(chatResponse);
                        
                        // 停止思考动画
                        ThinkingIndicator.Visibility = System.Windows.Visibility.Collapsed;
                        StopThinkingAnimation();
                        
                        // 流式添加AI回复
                        if (responseText?.Messages != null)
                        {
                            foreach (var message in responseText.Messages)
                            {
                                foreach (var content in message.Contents)
                                {
                                    // 模拟流式效果，每个消息间隔一小段时间
                                    await Task.Delay(100);
                                    if (string.IsNullOrEmpty(content.Text)) continue;
                                    AddAIMessage(content.Text);
                                }
                            }
                        }
                        else
                        {
                            // 如果没有消息，显示一个默认回复
                            AddAIMessage("抱歉，我没有找到合适的回复。");
                        }
                        
                        // 滚动到底部
                        MessageScrollViewer.ScrollToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                // 停止思考动画
                ThinkingIndicator.Visibility = System.Windows.Visibility.Collapsed;
                StopThinkingAnimation();
                
                // 显示错误消息
                AddAIMessage($"发生错误: {ex.Message}");
                MessageScrollViewer.ScrollToEnd();
            }
        }



        private string ConvertToString(Curve curve)
        {
            return $"Curve Data is : Start = {ConvertToString(curve.GetEndPoint(0))} , End = {ConvertToString(curve.GetEndPoint(1))}";
        }

        private string ConvertToString(XYZ point)
        {
            return $"X = {point.X * 304.8}, Y = {point.Y * 304.8}, Z = {point.Z * 304.8}";
        }
    }
}
