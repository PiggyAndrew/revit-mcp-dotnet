using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace RevitTest.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class StopSocketServerCommand : IExternalCommand
    {


        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                // 调用静态方法停止服务
                RevitSocketServerCommand.StopServer();

                TaskDialog.Show("Revit Socket服务", "Revit Socket服务已停止");
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("错误", $"停止Socket服务失败：{ex.Message}");
                return Result.Failed;
            }
        }
    }
}