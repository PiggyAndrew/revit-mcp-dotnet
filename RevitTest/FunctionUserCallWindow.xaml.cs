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

        private  async void Button_Click(object sender, RoutedEventArgs e)
        {
            var selections = uiDocument.Selection.GetElementIds();
            var selection = ElementId.InvalidElementId;
            if (selections.Any())
            {

                var ele = document.GetElement(selections.FirstOrDefault()) as Wall;

                var wallLocation = ele.Location as LocationCurve;
                var wallString = ConvertToString(wallLocation.Curve);
                this.userInput = $"WallId:{selection} , WallData: {wallString}";
            }

            this.TextResult.Text = userInput;
            await Chat();
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
                    httpClient.DefaultRequestHeaders.Add("Origin", "http://localhost");
                    
                    // 构建请求URL，传递用户输入
                    string requestUrl = $"/RevitTest?userInput={this.TextBox.Text}"; 
                    
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
                        foreach (var text in responseText?.Messages?.SelectMany(x => x?.Contents.Select(x=>x.Text)))
                        {
                            this.TextResult.Text +=text +"\n";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 显示错误信息
                this.TextResult.Text = $"Error: {ex.Message}";
                if (ex.InnerException != null)
                {
                    this.TextResult.Text += $"\nInner Exception: {ex.InnerException.Message}";
                }
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
