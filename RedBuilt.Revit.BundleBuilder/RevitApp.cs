using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RedBuilt.Revit.BundleBuilder
{
    public class RevitApp : IExternalApplication
    {
        static AddInId m_appId = new AddInId(new Guid("BD4B0625-4B95-44E9-BC9C-5F7EE6505AD1"));

        /// <summary>
        /// Revit shutdown method
        /// </summary>
        /// <param name="application">revit applicaton</param>
        /// <returns>status</returns>
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        /// <summary>
        /// Revit startup method
        /// </summary>
        /// <param name="application">revit applicaton</param>
        /// <returns>status</returns>
        public Result OnStartup(UIControlledApplication application)
        {
            string assemblyPath = Assembly.GetExecutingAssembly().Location;

            // Create ribbon panel
            RibbonPanel ribbonPanel;
            try
            {
                ribbonPanel = application.CreateRibbonPanel("RedBuilt", "BundleBuilder");
            }
            catch
            {
                ribbonPanel = application.CreateRibbonPanel("BundleBuilder");
            }

            // Create push button
            PushButtonData pushButtonData = new PushButtonData("bundlebutton", "Bundle", assemblyPath, "RedBuilt.Revit.BundleBuilder.RevitCommand");
            PushButton pushButton = ribbonPanel.AddItem(pushButtonData) as PushButton;
            
            pushButton.ToolTip = "Custom Build Bundles";

            return Result.Succeeded;
        }

    }
}
