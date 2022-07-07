using RedBuilt.Revit.BundleBuilder.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.Application.Reports
{
    public class TestReport
    {
        public static void Export()
        {
            StreamWriter sw = new StreamWriter(@"C:\RedBuilt\Revit\BundleBuilder\RedBuilt.Revit.BundleBuilder\Documents\TestReport.html");
            PrintHeader(sw);
            
            sw.WriteLine("<p>" + Settings.StartingPanel + "</p>");
            sw.WriteLine("<p>" + Settings.StartingDirection + "</p>");

            List<Panel> panelList = new List<Panel>(Project.Panels);
            //foreach(Panel panel in panelList)
            //{
            //    sw.WriteLine("<p>" + panel.Name + "</p>");
            //}
            sw.WriteLine("<p>" + Project.Bundles.Count + "</p>");

            PrintFooter(sw);
            sw.Close();
        }

        private static void PrintHeader(StreamWriter sw)
        {
            sw.WriteLine("<!DOCTYPE html>");
            sw.WriteLine("<html>");
            sw.WriteLine("<head");
            sw.WriteLine("</head>");
            sw.WriteLine("<body>");
        }

        private static void PrintFooter(StreamWriter sw)
        {
            sw.WriteLine("</body>");
            sw.WriteLine("</html>");
        }
    }
}
