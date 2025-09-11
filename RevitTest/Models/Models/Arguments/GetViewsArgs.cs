using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using BUD.Tuna.Revit.Extensions.Collection;
using Newtonsoft.Json;

namespace BUD.Drainage.Revit.Addin.Models.Arguments
{
    public class GetViewsArgs : FunctionArgumentsBase
    {
        /// <summary>
        /// 函数名称常量
        /// </summary>
        public const string FUNCTION_NAME = "get_views";

        public override RevitCommandResult Execute(Autodesk.Revit.DB.Document document)
        {
            var contentObj = document.GetElements<View>().Select(view => new { ID = view.Id.IntegerValue, Name = view.Name, ViewType =Enum.GetName(typeof(ViewType), view.ViewType), IsActiveView = document.ActiveView?.Id == view.Id, GenLevel = view.GenLevel?.Id.ToString() });

            return RevitCommandResult.Success(JsonConvert.SerializeObject(contentObj));
        }

        public override bool Validate()
        {
            throw new NotImplementedException();
        }
    }
}
