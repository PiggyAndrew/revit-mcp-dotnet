// See https://aka.ms/new-console-template for more information


using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol.Server;
using NET.Mcp.Server.Tools;

var builder = Host.CreateApplicationBuilder();
builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithTools<TestTools>();

await builder.Build().RunAsync();






