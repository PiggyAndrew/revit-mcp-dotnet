using Autodesk.Revit.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUD.Drainage.Revit.Addin.Models.Arguments
{
    public class GetCategoriesArgs : FunctionArgumentsBase
    {
        /// <summary>
        /// 函数名称常量
        /// </summary>
        public const string FUNCTION_NAME = "get_categories";


        public override RevitCommandResult Execute(Document document)
        {
            var results = new List<object>();
            foreach (Category category in document.Settings.Categories)
            {
                results.Add(new { Name= category.Name,Id=category.Id ,BuiltInCategory=category.BuiltInCategory});
            }

            return RevitCommandResult.Success(JsonConvert.SerializeObject(results));
        }

        public override bool Validate()
        {
            throw new NotImplementedException();
        }
    }
}
