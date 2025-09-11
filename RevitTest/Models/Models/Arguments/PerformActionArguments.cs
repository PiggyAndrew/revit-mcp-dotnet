using Autodesk.Revit.DB;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace BUD.Drainage.Revit.Addin.Models.Arguments
{

    /// <summary>
    /// 操作类型枚举
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ActionType
    {
        /// <summary>
        /// 隐藏元素
        /// </summary>
        Hide,
        
        /// <summary>
        /// 显示元素
        /// </summary>
        Show,
        
        /// <summary>
        /// 隔离显示元素
        /// </summary>
        Isolate,
        
        /// <summary>
        /// 选择元素
        /// </summary>
        Select
    }
    /// <summary>
    /// 执行操作的参数类
    /// </summary>
    public class PerformActionArguments : FunctionArgumentsBase
    {
        /// <summary>
        /// 函数名称常量
        /// </summary>
        public const string FUNCTION_NAME = "perform_revit_action";

        /// <summary>
        /// 获取函数名称
        /// </summary>
        public  string FunctionName => FUNCTION_NAME;

/// <summary>
        /// 操作类型：Hide, Show, Isolate, Select
        /// </summary>
        [JsonProperty("action")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ActionType Action { get; set; }

        /// <summary>
        /// 目标类型：element, category, view, parameter
        /// </summary>
        [JsonProperty("target_type")]
        public string TargetType { get; set; }

        /// <summary>
        /// 操作目标列表
        /// </summary>
        [JsonProperty("targets")]
        public List<string> Targets { get; set; }

     

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="document">Revit文档</param>
        /// <returns>执行结果</returns>
        public override RevitCommandResult Execute(Document document)
        {
            throw new NotImplementedException();
        }

        public override bool Validate()
        {
            throw new NotImplementedException();
        }
    }
}