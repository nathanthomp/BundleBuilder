using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RedBuilt.Revit.BundleBuilder.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder
{
    /// <summary>
    /// Interaction logic for Revit
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class RevitCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = uiApp.Application;
            Document doc = uiDoc.Document;

            try
            {
                MainWindow mw = new MainWindow(doc);
                mw.ShowDialog();
                return Result.Succeeded;
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                TaskDialog.Show("BundleBuilder", "This operation was cancelled");
                return Result.Cancelled;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("BundleBuilder", "The following error occured: " + ex.Message);
                return Result.Failed;
            }
        }
    }
}
