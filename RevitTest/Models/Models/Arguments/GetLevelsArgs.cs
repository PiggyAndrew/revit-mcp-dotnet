using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using BUD.Tuna.Revit.Extensions.Collection;
using BUD.Tuna.Revit.Extensions.Extensions;
using Newtonsoft.Json;

namespace BUD.Drainage.Revit.Addin.Models.Arguments
{
    public class GetLevelsArgs : FunctionArgumentsBase
    {
        /// <summary>
        /// 函数名称常量
        /// </summary>
        public const string FUNCTION_NAME = "get_levels";


        public override RevitCommandResult Execute(Document document)
        {
            var levels = document.GetElements<Level>();
            var contentObj = levels.Select(level => new { Name = level.Name, Elevation = level.Elevation.ConvertToMillimeters(), Unit = "毫米",ID=level.Id.IntegerValue });

            return RevitCommandResult.Success(JsonConvert.SerializeObject(contentObj));
        }

        public override bool Validate()
        {
            throw new NotImplementedException();
        }
    }
}
