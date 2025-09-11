using BUD.Drainage.Revit.Addin.Models.Arguments;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace BUD.Drainage.Revit.Addin.Models
{
    /// <summary>
    /// 单个命令
    /// </summary>
    public class RevitCommandModel
    {
        /// <summary>
        /// 消息类型，固定为"revitCommand"
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// 工具调用数组
        /// </summary>
        [JsonProperty("toolCalls")]
        public List<ToolCallModel> ToolCalls { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        /// <summary>
        /// 获取格式化的时间戳
        /// </summary>
        /// <returns>DateTime对象</returns>
        public DateTime GetTimestamp()
        {
            if (DateTime.TryParse(Timestamp, out DateTime result))
            {
                return result;
            }
            return DateTime.Now;
        }
    }


      /// <summary>
    /// 工具调用类，对应Web端的ToolCall.js
    /// </summary>
    public class ToolCallModel
    {
        /// <summary>
        /// 工具调用的唯一标识符
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// 工具调用的类型，通常为"function"
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// 函数对象
        /// </summary>
        [JsonProperty("function")]
        public FunctionModel Function { get; set; }

        /// <summary>
        /// 函数对象
        /// </summary>
        [JsonProperty("result")]
        public RevitCommandResult Result { get; set; }
    }

/// <summary>
    /// 函数调用类，对应Web端的Function.js
    /// </summary>
    public class FunctionModel
    {
        /// <summary>
        /// 函数名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 函数参数，使用JObject类型以支持嵌套的JSON对象结构
        /// </summary>
        [JsonProperty("arguments")]
        public JObject Arguments { get; set; }
    }



 /// <summary>
    /// 用于解析Web端发送的命令的工具类
    /// </summary>
    public static class CommandParser
    {
        /// <summary>
        /// 从JSON字符串解析命令
        /// </summary>
        /// <param name="jsonString">JSON字符串</param>
        /// <returns>解析后的命令对象</returns>
        public static RevitCommandModel ParseCommand(string jsonString)
        {
            try
            {
                return JsonConvert.DeserializeObject<RevitCommandModel>(jsonString);
            }
            catch (Exception ex)
            {
                throw new Exception($"解析命令失败: {ex.Message}", ex);
            }
        }
    }
}