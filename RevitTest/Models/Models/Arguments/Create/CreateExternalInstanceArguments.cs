using Autodesk.Revit.DB;
using BUD.Tuna.Revit.Extensions.Collection;
using BUD.Tuna.Revit.Extensions.Transaction;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace BUD.Drainage.Revit.Addin.Models.Arguments.Create
{
    /// <summary>
    /// 创建实例的参数类
    /// </summary>
    public class CreateExternalInstanceArguments : FunctionArgumentsBase
    {
        /// <summary>
        /// 函数名称常量
        /// </summary>
        public const string FUNCTION_NAME = "create_revit_external_instance";

        /// <summary>
        /// 获取函数名称
        /// </summary>
        public string FunctionName => FUNCTION_NAME;

        /// <summary>
        /// 要创建的元素类型
        /// </summary>
        [JsonProperty("element_type")]
        public string ElementType { get; set; } = "";

        /// <summary>
        /// 族名称，对于族实例是必需的
        /// </summary>
        [JsonProperty("family_name")]
        public string FamilyName { get; set; } = "";

        /// <summary>
        /// 类型名称，如墙类型或族类型名称
        /// </summary>
        [JsonProperty("type_name")]
        public string TypeName { get; set; } = "";

        /// <summary>
        /// 放置数据
        /// </summary>
        [JsonProperty("placement")]
        public PlacementData Placement { get; set; }

        /// <summary>
        /// 尺寸信息
        /// </summary>
        [JsonProperty("dimensions")]
        public DimensionsData Dimensions { get; set; }

        /// <summary>
        /// 元素参数键值对
        /// </summary>
        [JsonProperty("parameters")]
        public Dictionary<string, object> Parameters { get; set; }

        public override RevitCommandResult Execute(Document document)
        {
            try
            {
                // 1. 查找族和族类型
                FamilySymbol familySymbol = FindFamilySymbol(document);
                if (familySymbol == null)
                {
                    return RevitCommandResult.Error($"创建失败: 未找到对应的族类型{TypeName}");
                }

                Level level = document.ActiveView.GenLevel;
                if (level==null)
                {
                   level= document.GetElements<Level>(x => x.IsValidObject).FirstOrDefault();
                }
                Family family = familySymbol.Family;
                var hostType = (FamilyHostingBehavior)family.get_Parameter(BuiltInParameter.FAMILY_HOSTING_BEHAVIOR).AsInteger();
                var results = new List<Element>();    
                var transactionResult = document.NewTransaction(() =>
                 {
                     if (!familySymbol.IsActive)
                     {
                         familySymbol.Activate();
                     }
                     foreach (var point in Placement.Points)
                     {
                         Element element = null;
                         switch (hostType)
                         {
                             case FamilyHostingBehavior.None:
                                 element=document.Create.NewFamilyInstance(point, familySymbol, level, Placement.StructuralType);
                                 break;
                             case FamilyHostingBehavior.Wall:
                                 break;
                             case FamilyHostingBehavior.Floor:
                                 break;
                             case FamilyHostingBehavior.Ceiling:
                                 break;
                             case FamilyHostingBehavior.Roof:
                                 break;
                             case FamilyHostingBehavior.Face:
                                 break;
                             default:
                                 break;
                         }
                         if (element != null)
                         {
                             results.Add(element);
                         }
                     }
                   
                 }, $"Create {familySymbol.FamilyName}");
                if (transactionResult.HasException)
                {
                    throw transactionResult.Exception;
                }
                return RevitCommandResult.SuccessWithElements($"创建成功", results);
            }
            catch (Exception ex)
            {
                return RevitCommandResult.Error($"创建失败: {ex.Message}");
            }
        }


        public FamilySymbol FindFamilySymbol(Document document)
        {
            var options = new List<FamilySymbol>();
            BuiltInCategory builtInCategory = BuiltInCategory.INVALID;
            switch (ElementType)
            {
                case "column":
                case "柱":
                    builtInCategory = BuiltInCategory.OST_StructuralColumns;
                    break;
                default:
                    break;
            }
            options = document.GetElements<FamilySymbol>(x => (BuiltInCategory)x.Category.Id.IntegerValue== builtInCategory).ToList();
            if (!options.Any())
            {
                return null;
            }

            FamilySymbol familySymbol = options.Where(x => x.FamilyName.Contains(FamilyName)).FirstOrDefault();
            if (familySymbol != null)
            {
                return familySymbol;
            }

            // 1. 查找族和族类型
            familySymbol = options.Where(x => x.Name.Contains(TypeName)).FirstOrDefault();
            if (familySymbol != null)
            {
                return familySymbol;
            }
            return options.FirstOrDefault();
        }


        /// <summary>
        /// 验证参数是否有效
        /// </summary>
        /// <returns>验证结果</returns>
        public override bool Validate()
        {
            if (string.IsNullOrEmpty(ElementType))
            {
                ErrorMessage = "元素类型不能为空";
                return false;
            }

            if (Placement == null)
            {
                ErrorMessage = "放置数据不能为空";
                return false;
            }

            return true;
        }
    }
}