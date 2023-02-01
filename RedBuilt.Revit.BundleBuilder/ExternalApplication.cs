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
    public class ExternalApplication : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            AddMenu(application);
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public void AddMenu(UIControlledApplication application)
        {
            string assemblyPath = Assembly.GetExecutingAssembly().Location;

            RibbonPanel ribbonPanel;
            try
            {
                ribbonPanel = application.CreateRibbonPanel("RedBuilt", "Bundle");
            }
            catch
            {
                ribbonPanel = application.CreateRibbonPanel("Bundle");
            }

            PulldownButtonData data = new PulldownButtonData("Options", "BundleBuilder");
            
            RibbonItem item = ribbonPanel.AddItem(data);
            PulldownButton optionsButton = item as PulldownButton;
            
            optionsButton.AddPushButton(new PushButtonData("Bundle", "Bundle", assemblyPath, typeof(BundleCommand).FullName));
            optionsButton.AddPushButton(new PushButtonData("Version", "Version", assemblyPath, typeof(VersionCommand).FullName));
            optionsButton.ToolTip = "Custom Build Bundles";
            optionsButton.LargeImage = GetEmbeddedImage("RedBuilt.Revit.BundleBuilder.Resources.BundleBuilder.ico");
        }

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
