using BUD.Drainage.Revit.Addin.Models.Arguments;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BUD.Drainage.Revit.Addin.Models
{
    /// <summary>
    /// 函数参数工厂类
    /// </summary>
    public static class FunctionArgumentsFactory
    {
        // 缓存函数名称与参数类型的映射关系
        private static readonly Dictionary<string, Type> _functionTypeMap;

        /// <summary>
        /// 静态构造函数，初始化函数名称与类型的映射
        /// </summary>
        static FunctionArgumentsFactory()
        {
            _functionTypeMap = new Dictionary<string, Type>();

            // 获取当前程序集中所有继承自FunctionArgumentsBase的类型
            var types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(FunctionArgumentsBase).IsAssignableFrom(t));

            foreach (var type in types)
            {
                // 获取类中的FUNCTION_NAME常量
                var fieldInfo = type.GetField("FUNCTION_NAME", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (fieldInfo != null && fieldInfo.IsLiteral && !fieldInfo.IsInitOnly)
                {
                    string functionName = fieldInfo.GetValue(null) as string;
                    if (!string.IsNullOrEmpty(functionName))
                    {
                        _functionTypeMap[functionName] = type;
                    }
                }
            }
        }

        /// <summary>
        /// 根据函数模型创建对应的参数类实例
        /// </summary>
        /// <param name="functionModel">函数模型</param>
        /// <returns>参数类实例</returns>
        public static FunctionArgumentsBase Create(FunctionModel functionModel)
        {
            if (functionModel == null)
                throw new ArgumentNullException(nameof(functionModel), "函数模型不能为空");

            return Create(functionModel.Name, functionModel.Arguments);
        }


        /// <summary>
        /// 根据函数名称创建对应的参数类实例
        /// </summary>
        /// <param name="functionName">函数名称</param>
        /// <param name="arguments">参数JSON对象</param>
        /// <returns>参数类实例</returns>
        public static FunctionArgumentsBase Create(string functionName, dynamic arguments)
        {
            string json = JsonConvert.SerializeObject(arguments);

            if (_functionTypeMap.TryGetValue(functionName, out Type type))
            {
                return (FunctionArgumentsBase)JsonConvert.DeserializeObject(json, type);
            }

            throw new ArgumentException($"不支持的函数名称: {functionName}");
        }
    }
}