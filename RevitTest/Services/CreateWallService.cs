using Autodesk.Revit.DB;
using Newtonsoft.Json.Linq;
using RevitTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitTest.Services
{
    public class CreateWallService
    {
        private Document _document;

        public CreateWallService(Document document) { _document = document; }


        public  Element Execute(JObject args)
        {
            var data = args.ToObject<CreateWallData>();
            if (string.IsNullOrEmpty(data.Command)) return null;
            if (data.Command == "CreateWall")
            {
                var startPoint = new XYZ(data.Args.Start[0], data.Args.Start[1], data.Args.Start[2]);
                var endPoint = new XYZ(data.Args.End[0], data.Args.End[1], data.Args.End[2]);
                //json转curve
                var curve = Line.CreateBound(startPoint, endPoint);
                IList<Curve> curves = [curve];
                //正式执行

                return  Wall.Create(_document, curves, false);
            }
            return null;
        }
    }
}
