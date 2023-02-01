using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RedBuilt.Revit.BundleBuilder.Data.Services;
using RedBuilt.Revit.BundleBuilder.Data.States;
using RedBuilt.Revit.BundleBuilder.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RedBuilt.Revit.BundleBuilder
{
    [Transaction(TransactionMode.Manual)]
    public class RevitCommand : IExternalCommand
    {
        /// <summary>
        /// Revit plugin interaction logic
        /// </summary>
        /// <param name="commandData">revit command data</param>
        /// <param name="message">revit status message</param>
        /// <param name="elements">elements</param>
        /// <returns>status</returns>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            ProjectState.UIApp = uiApp;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            ProjectState.UIDoc = uiDoc;
            Autodesk.Revit.ApplicationServices.Application app = uiApp.Application;
            ProjectState.App = app;
            Document doc = uiDoc.Document;
            ProjectState.Doc = doc;

            // Attempt to create data
            try
            {
                RevitImportService.Import(doc);
            }
            catch (Exception ex)
            {
                message = RevitImportService.ErrorMessage + ": " + ex.Message;
                return Result.Failed;
            }

            // Attempt to create windows
            try
            {
                MainWindow mw = new MainWindow(doc);
                mw.ShowDialog();
                return Result.Succeeded;
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                // TaskDialog.Show("BundleBuilder", "This operation was cancelled");
                message = "This operation was cancelled";
                return Result.Cancelled;
            }
            catch (Exception ex)
            {
                // TaskDialog.Show("BundleBuilder", "The following error occured: " + ex.Message);
                message = "The following error occured: " + ex.Message;
                return Result.Failed;
            }
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Version : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            MessageBox.Show("BundleBuilder Revision: " + ProjectState.Version);
            return Result.Succeeded;
        }
    }
}
