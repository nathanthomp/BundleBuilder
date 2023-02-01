using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

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

            // Attempt to create ribbon panel
            // When referencing the RedBuilt tab...
            // the .addin file of this .dll must contain the prefix RedBuilt,
            // this is because the .addin file for the .dll containing the RedBuilt
            // tab is named RedBuilt and .addin files are loaded alphabetically
            RibbonPanel ribbonPanel;
            try
            {
                ribbonPanel = application.CreateRibbonPanel("RedBuilt", "Bundle");
            }
            catch
            {
                ribbonPanel = application.CreateRibbonPanel("Bundle");
            }

            // Create pull down button
            PulldownButtonData data = new PulldownButtonData("Options", "BundleBuilder");
            
            RibbonItem item = ribbonPanel.AddItem(data);
            PulldownButton optionsButton = item as PulldownButton;
            
            optionsButton.AddPushButton(new PushButtonData("bundlebutton", "BundleBuilder", assemblyPath, "RedBuilt.Revit.BundleBuilder.RevitCommand"));
            optionsButton.AddPushButton(new PushButtonData("versionbutton", "Version", assemblyPath, "RedBuilt.Revit.BundleBuilder.Version"));

            // Create push button
            // PushButtonData pushButtonData = new PushButtonData("bundlebutton", "BundleBuilder", assemblyPath, "RedBuilt.Revit.BundleBuilder.RevitCommand");


            // PushButton pushButton = ribbonPanel.AddItem(pushButtonData) as PushButton;

            optionsButton.ToolTip = "Custom Build Bundles";
            optionsButton.LargeImage = GetEmbeddedImage("RedBuilt.Revit.BundleBuilder.Resources.BundleBuilder.ico");

            return Result.Succeeded;
        }

        /// <summary>
        /// Gets embedded image source
        /// </summary>
        /// <param name="name">name of image file</param>
        /// <returns>image source, otherwise null</returns>
        static BitmapFrame GetEmbeddedImage(string name)
        {
            try
            {
                Assembly a = Assembly.GetExecutingAssembly();
                Stream s = a.GetManifestResourceStream(name);
                return BitmapFrame.Create(s);
            }
            catch
            {
                return null;
            }
        }

    }
}
