// See https://aka.ms/new-console-template for more information


using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;
using OpenAI.Chat;
using OpenAI;
using System.ClientModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;
using Newtonsoft.Json;
using System.Windows.Forms;

var input = string.Join("查看revit command mcp", args);

Debug.Print(input);
await using var mcpClient = await McpClientFactory.CreateAsync(new StdioClientTransport(new StdioClientTransportOptions()
{
    Name = "Demo Server",
    Command = "powershell",
    Arguments = ["D:\\GitHub\\revit-mcp-dotnet\\NET.Mcp.Server\\bin\\Debug\\net8.0\\NET.Mcp.Server.exe"]
}));

var openAiOptions = new OpenAIClientOptions();
openAiOptions.Endpoint = new Uri("https://api.deepseek.com/v1/");


var chatClient = new ChatClient("deepseek-chat", new ApiKeyCredential("sk-58c16b28eacc41349e6ffd875d7c914a"), openAiOptions);

var client = new ChatClientBuilder(chatClient.AsIChatClient()).UseFunctionInvocation().Build();

var prompts = new List<Microsoft.Extensions.AI.ChatMessage>
{
    new ChatMessage(ChatRole.System, """you are a professional enginer in BIM , so you can select the greate tool to user , and generation a standard input style And Arguments to tools"""),
    new ChatMessage(ChatRole.User, input)
};


var tools = await mcpClient.ListToolsAsync();


var chatOptions = new ChatOptions()
{
    Tools = [.. tools]
};
var res = await client.GetResponseAsync(prompts, chatOptions);

var message = res.Messages[0].Contents[0];
//var value = ((Microsoft.Extensions.AI.FunctionResultContent)message).Result;
var convert = JsonConvert.DeserializeObject(message.ToString());

// 反序列化
ResponseData data = JsonConvert.DeserializeObject<ResponseData>(message.ToString());

// 访问数据
foreach (var item in data.Content)
{
    var d = item.Text;
    
    Console.WriteLine(d);
}

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
