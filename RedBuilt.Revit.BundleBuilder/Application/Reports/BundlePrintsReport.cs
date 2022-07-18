using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout;
using RedBuilt.Revit.BundleBuilder.Data.Models;
using RedBuilt.Revit.BundleBuilder.Data.States;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RedBuilt.Revit.BundleBuilder.Application.Reports
{
    public class BundlePrintsReport
    {
        private static readonly string localFile = @"C:\RedBuilt\Revit\BundleBuilder\RedBuilt.Revit.BundleBuilder\Documents\BundlePrints.html";
        private static readonly string imageFile = @"C:\RedBuilt\Revit\BundleBuilder\RedBuilt.Revit.BundleBuilder\Resources\Built.svg";

        public static void CreatePdf(string fileName)
        {
            #region Convert HTML to PDF
            try
            {
                CreateHtml();
            }
            catch
            {
                throw new Exception("Cannot create HTML file - BundlePrints");
            }

            using (FileStream htmlSource = File.Open(localFile, FileMode.Open))
            using (FileStream pdfDest = File.Open(fileName, FileMode.Create))
            {
                ConverterProperties converterProperties = new ConverterProperties();
                HtmlConverter.ConvertToPdf(htmlSource, pdfDest, converterProperties);
            }
            #endregion
        }

        private static void CreateHtml()
        {
            StreamWriter sw = new StreamWriter(localFile);

            CreateFileHeader(sw);

            foreach (Bundle bundle in Project.Bundles)
            {
                CreatePage(bundle, sw);

                // Remove after test //
                break;
            }

            CreateFileFooter(sw);
        }

        private static void CreateFileHeader(StreamWriter sw)
        {
            sw.WriteLine("<html>");
            sw.WriteLine("<head>");
            sw.WriteLine("<title>BundlePrints</title>");
            sw.WriteLine("<style>");

            sw.WriteLine("body{ margin-top: 0px; }");
            sw.WriteLine(".page{ font-size: 18px;}");

            // Header
            sw.WriteLine("header{ column-count: 2; }");
            sw.WriteLine("img{ width: 240px; height: 72px; }");
            sw.WriteLine(".version{ padding-top: 30px; text-align: right; }");

            // Bundle Info
            sw.WriteLine(".bundle_info{ display: grid; padding-top: 40px; padding-bottom: 60px; }");
            sw.WriteLine(".bundle_info-left-side-title{ grid-column-start: 1; }");
            sw.WriteLine(".bundle_info-left-side-data{ grid-column-start: 2; }");
            sw.WriteLine(".bundle_info-right-side-title{ grid-column-start: 3; }");
            sw.WriteLine(".bundle_info-right-side-data{ grid-column-start: 4; }");
            sw.WriteLine(".row{ padding: 5px; }");
            sw.WriteLine(".row-1{ grid-row-start: 1; }");
            sw.WriteLine(".row-2{ grid-row-start: 2; }");
            sw.WriteLine(".row-3{ grid-row-start: 3; }");
            sw.WriteLine(".row-4{ grid-row-start: 4; }");

            // Views
            sw.WriteLine(".length-view{ padding-bottom: 40px; height: 250px; }");
            sw.WriteLine(".width-view{ padding-bottom: 20px; height: 250px; }");
            sw.WriteLine(".view-title{ text-align: center; padding-bottom: 20px; }");
            sw.WriteLine(".panel{ border: 1px solid black; }");
            sw.WriteLine(".sticker{ height: 6px; }");

            // Footer
            sw.WriteLine("footer{ column-count: 3; text-align: center; padding-top: 90px; }");
            sw.WriteLine(".footer-project{ text-align: left; }");
            sw.WriteLine(".footer-date_created{ text-align: center; }");
            sw.WriteLine(".footer-page_index{ text-align: right; }");

            sw.WriteLine("</style>");
            sw.WriteLine("</head>");
            sw.WriteLine("<body>");
        }

        private static void CreatePage(Bundle bundle, StreamWriter sw)
        {
            sw.WriteLine("<div class=\"page\">");

            // Header
            sw.WriteLine("\t<header><img src=\"" + imageFile + "\"/><div> class=\"version\">BundleBuilder v" + ProjectState.Version + "</div></header>");

            // Bundle Info
            sw.WriteLine("\t<div class=\"bundle_info\">");
            sw.WriteLine("\t\t<div class=\"col bundle_info-left-side-title\">");
            sw.WriteLine("\t\t\t<div class=\"row row-1\">Project Name</div>");
            sw.WriteLine("\t\t\t<div class=\"row row-2\">Project Number</div>");
            sw.WriteLine("\t\t\t<div class=\"row row-3\">Project Location</div>");
            sw.WriteLine("\t\t\t<div class=\"row row-4\">Bundle</div>");
            sw.WriteLine("\t\t</div>");

            sw.WriteLine("\t\t<div class=\"col bundle_info-left-side-data\">");
            sw.WriteLine("\t\t\t<div class=\"row row-1\">" + Project.Name + "</div>");
            sw.WriteLine("\t\t\t<div class=\"row row-2\">" + Project.Number + "</div>");
            sw.WriteLine("\t\t\t<div class=\"row row-3\">" + Project.Location + "</div>");
            sw.WriteLine("\t\t\t<div class=\"row row-4\">" + bundle.ToString() + "</div>");
            sw.WriteLine("\t\t</div>");

            sw.WriteLine("\t\t<div class=\"col bundle_info-right-side-Title\">");
            sw.WriteLine("\t\t\t<div class=\"row row-1\">Bundle Length</div>");
            sw.WriteLine("\t\t\t<div class=\"row row-2\">Bundle Width</div>");
            sw.WriteLine("\t\t\t<div class=\"row row-3\">Bundle Height</div>");
            sw.WriteLine("\t\t\t<div class=\"row row-4\">Bundle Weight</div>");
            sw.WriteLine("\t\t</div>");

            sw.WriteLine("\t\t<div class=\"col bundle_info-left-side-data\">");
            sw.WriteLine("\t\t\t<div class=\"row row-1\">" + bundle.Length + "</div>");
            sw.WriteLine("\t\t\t<div class=\"row row-2\">" + bundle.Width + "</div>");
            sw.WriteLine("\t\t\t<div class=\"row row-3\">" + bundle.Height + "</div>");
            sw.WriteLine("\t\t\t<div class=\"row row-4\">" + bundle.Weight + "</div>");
            sw.WriteLine("\t\t</div>");

            sw.WriteLine("\t</div>");

            // Length View
            sw.WriteLine("\t<div class=\"length-view\">");
            sw.WriteLine("\t\t<div class=\"view-title\">Length View</div>");
            sw.WriteLine("\t\t<div class=\"view-bundle\">");

            foreach (Level level in bundle.Levels)
            {

                sw.WriteLine("\t\t\t<div class=\"panel\" style=\"width: " + (level.Length * 2.25) + 6 + "px; height: " + (level.Height) + 6 + "px;\">");

                foreach (Panel panel in level.Panels)
                {
                    sw.Write(panel.Name.FullName + " ");
                }

                sw.Write("</div>");
                sw.WriteLine("\t\t\t<div class=\"sticker\"></div>");
            }
            sw.WriteLine("\t\t</div>");
            sw.WriteLine("\t</div>");

            // Width View
            sw.WriteLine("\t<div class=\"width-view\">");
            sw.WriteLine("\t\t<div class=\"view-title\">Width View</div>");
            sw.WriteLine("\t\t<div class=\"view-bundle\">");

            foreach (Level level in bundle.Levels)
            {
                sw.WriteLine("\t\t\t<div class=\"panel\" style=\"width: " + (level.Width * 2.25) + 6 + "px; height: " + (level.Height) + 6 + "px;\"></div>");
                sw.WriteLine("\t\t\t<div class=\"sticker\"></div>");
            }

            sw.WriteLine("\t\t</div>");
            sw.WriteLine("\t</div>");

            // Footer
            sw.WriteLine("\t<footer>");
            sw.WriteLine("\t\t<div class=\"footer-project\">" + Project.Number + "</div>");
            sw.WriteLine("\t\t<div class=\"footer-date_created\">" + DateTime.Now.ToShortDateString() + "</div>");
            sw.WriteLine("\t\t<div class=\"footer-page_index\">Bundle" + bundle.Number + "/" + Project.Bundles.Count + "</div>");
            sw.WriteLine("\t</footer>");

            sw.WriteLine("/<div>");
        }

        private static void CreateFileFooter(StreamWriter sw)
        {
            sw.WriteLine("</body>");
            sw.WriteLine("</html>");
        }
    }
}
