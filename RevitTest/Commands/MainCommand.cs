using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace RevitTest.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class MainCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var modelessView = new ChatWindow(commandData);

            System.Windows.Interop.WindowInteropHelper mainUI = new System.Windows.Interop.WindowInteropHelper(modelessView);
            mainUI.Owner = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
            modelessView.Show();
            return Result.Succeeded;
        }
    }


}
