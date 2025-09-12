using System.ClientModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;
using OpenAI;
using OpenAI.Chat;

namespace NET.MCP.WebAPI.Revit.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RevitTestController : ControllerBase
    {
    private readonly ILogger<RevitTestController> _logger;

        public RevitTestController(ILogger<RevitTestController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "Test")]
        public async Task<ChatResponse> Test([FromQuery]string userInput)
        {
            var mcpClient = await McpClientFactory.CreateAsync(new StdioClientTransport(new StdioClientTransportOptions()
            {
                Name = "Demo Server",
                Command = "powershell",
                Arguments = ["D:\\Gitspace\\revit-mcp-dotnet\\NET.Mcp.Server\\bin\\Debug\\net8.0\\NET.Mcp.Server.exe"]
            }));
            var openAiOptions = new OpenAIClientOptions();
            openAiOptions.Endpoint = new Uri("https://api.deepseek.com/v1/");


            var chatClient = new ChatClient("deepseek-chat", new ApiKeyCredential("sk-58c16b28eacc41349e6ffd875d7c914a"), openAiOptions);

            var client = new ChatClientBuilder(chatClient.AsIChatClient()).UseFunctionInvocation().Build();


            var prompts = new List<Microsoft.Extensions.AI.ChatMessage>
{
    new Microsoft.Extensions.AI.ChatMessage(ChatRole.System, """you are a professional enginer in BIM , so you can select the greate tool to user , and generation a standard input style And Arguments to tools"""),
    new Microsoft.Extensions.AI.ChatMessage(ChatRole.User, userInput)
};
            var tools = await mcpClient.ListToolsAsync();

            var chatOptions = new ChatOptions()
            {
                Tools = [.. tools]
            };
            var res = await client.GetResponseAsync(prompts, chatOptions);

            return res;
        }
    }
}
