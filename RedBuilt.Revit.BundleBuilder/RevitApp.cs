using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder
{
    public class RevitApp : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            AddMenu(application);
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public void AddMenu(UIControlledApplication app)
        {
            
            string assemblyPath = Assembly.GetExecutingAssembly().Location;
            //string commandPath = typeof(RevitApp).Namespace + "." + nameof(RevitCommand);
            string commandPath = "RedBuilt.Revit.BundleBuilder.RevitCommand";

            PushButtonData pushButtonData = new PushButtonData("BundleButton", "BundleBuilder", assemblyPath, commandPath);
            RibbonPanel ribbonPanel = app.CreateRibbonPanel("RedBuilt", "BundleBuilder");

            PushButton pushButton = ribbonPanel.AddItem(pushButtonData) as PushButton;
        }
    }
}
