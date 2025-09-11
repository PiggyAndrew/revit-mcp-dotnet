using Autodesk.Revit.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUD.Drainage.Revit.Addin.Models.Arguments
{
    public class GetElementsArgs : FunctionArgumentsBase
    {
        /// <summary>
        /// 函数名称常量
        /// </summary>
        public const string FUNCTION_NAME = "get_elements";

        [JsonProperty("levelIds")]
        public List<ElementId> LevelIds { get; set; }

        [JsonProperty("categoryIds")]
        public List<ElementId> CategoryIds { get; set; }

        [JsonProperty("ViewIds")]
        public List<ElementId> ViewIds { get; set; }

        public override RevitCommandResult Execute(Document document)
        {

            return RevitCommandResult.Success("Success");
        }

        public override bool Validate()
        {
            throw new NotImplementedException();
        }
    }
}
