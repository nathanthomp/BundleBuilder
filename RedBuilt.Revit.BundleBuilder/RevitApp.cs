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
            app.CreateRibbonPanel("RedBuilt", "BundleBuilder");
            string assemblyPath = Assembly.GetExecutingAssembly().Location;
            string commandPath = typeof(RevitApp).Namespace + "." + nameof(RevitApp);
        }
    }
}
