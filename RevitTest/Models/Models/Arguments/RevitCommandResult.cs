using Autodesk.Revit.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUD.Drainage.Revit.Addin.Models.Arguments
{
    /// <summary>
    /// 命令执行结果
    /// </summary>
    public class RevitCommandResult
    {
        /// <summary>
        /// 执行状态，成功为"success"，失败为"error"
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// 结果消息
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// 详细信息
        /// </summary>
        [JsonProperty("content")]
        public object Content { get; set; }


        /// <summary>
        /// 详细信息
        /// </summary>
        [JsonProperty("hasElements")]
        public bool HasElements { get; set; }


        /// <summary>
        /// 创建成功结果
        /// </summary>
        /// <param name="message">成功消息</param>
        /// <param name="content">详细信息</param>
        /// <returns>成功的命令结果</returns>
        public static RevitCommandResult Success(string message, Dictionary<string, object> content = null)
        {
            return new RevitCommandResult
            {
                Status = "success",
                Message = message,
                Content = content ?? new Dictionary<string, object>()
            };
        }



        /// <summary>
        /// 创建成功结果
        /// </summary>
        /// <param name="message">成功消息</param>
        /// <param name="content">详细信息</param>
        /// <returns>成功的命令结果</returns>
        public static RevitCommandResult Success(string content)
        {
            return new RevitCommandResult
            {
                Status = "success",
                Content = content
            };
        }


        /// <summary>
        /// 创建成功结果
        /// </summary>
        /// <param name="message">成功消息</param>
        /// <param name="content">详细信息</param>
        /// <returns>成功的命令结果</returns>
        public static RevitCommandResult SuccessWithElements(string message, List<Element> elements)
        {
            var content=new Dictionary<string, object>();
            content.Add("elementIds", elements.Select(x => x.Id).ToArray());

            return new RevitCommandResult
            {
                Status = "success",
                Message = message,
                Content = content ?? new Dictionary<string, object>(),
                HasElements=true
            };
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="content">详细信息</param>
        /// <returns>失败的命令结果</returns>
        public static RevitCommandResult Error(string message, Dictionary<string, object> content = null)
        {
            return new RevitCommandResult
            {
                Status = "error",
                Message = message,
                Content = content ?? new Dictionary<string, object>()
            };
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        [JsonIgnore]
        public bool IsSuccess => Status == "success";
    }
}
