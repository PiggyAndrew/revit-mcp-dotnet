using Autodesk.Revit.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BUD.Drainage.Revit.Addin.Models.Arguments
{
    /// <summary>
    /// 函数参数基类
    /// </summary>
    public abstract class FunctionArgumentsBase
    {


        /// <summary>
        /// 函数名称常量
        /// </summary>
        public const string FUNCTION_NAME = "perform_revit_action";

        /// <summary>
        /// 获取函数名称
        /// </summary>
        public string FunctionName => FUNCTION_NAME;


        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }


        /// <summary>
        /// 执行函数操作
        /// </summary>
        /// <param name="document">Revit文档</param>
        /// <returns>执行结果</returns>
        public abstract RevitCommandResult Execute(Document document);

        /// <summary>
        /// 验证参数是否有效
        /// </summary>
        /// <returns>验证结果，如果有效返回true，否则返回false</returns>
        public abstract bool Validate();

    }

       


}