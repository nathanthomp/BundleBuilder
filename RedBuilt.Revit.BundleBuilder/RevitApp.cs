using Autodesk.Revit.DB;
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
        static AddInId m_appId = new AddInId(new Guid("BD4B0625-4B95-44E9-BC9C-5F7EE6505AD1"));

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            AddMenu(application);
            return Result.Succeeded;
        }

        private void AddMenu(UIControlledApplication application)
        {
            application.CreateRibbonTab("BundleBuilderTest");
            string assemblyPath = Assembly.GetExecutingAssembly().Location;

            RibbonPanel ribbonPanel = application.CreateRibbonPanel("BundleBuilderTest", "BundleBuilder");

            PushButtonData pushButtonData = new PushButtonData("BundleButton", "BundleBuilder", assemblyPath, typeof(RevitCommand).FullName);
            RibbonItem item = ribbonPanel.AddItem(pushButtonData);

            item.ToolTip = "Custom Build Bundles";

        }
    }
}
