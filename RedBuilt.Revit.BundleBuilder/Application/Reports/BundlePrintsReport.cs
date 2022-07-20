using iText.Html2pdf;
using iText.Kernel.Geom;
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
        private static readonly string imageFile = @"C:\RedBuilt\Revit\BundleBuilder\RedBuilt.Revit.BundleBuilder\Resources\RedBuilt.svg";

        public static void CreatePdf(string fileName)
        {

            if (File.Exists(fileName))
                File.Delete(fileName);
            if (File.Exists(localFile))
                File.Delete(localFile);

            CreateHtml();

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
            }
                

            CreateFileFooter(sw);

            sw.Close();
        }

        private static void CreateFileHeader(StreamWriter sw)
        {
            sw.WriteLine("<!DOCTYPE html>");
            sw.WriteLine("<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">");
            sw.WriteLine("\t<head>");
            sw.WriteLine("\t\t<meta charset=\"utf-8\"/>");
            sw.WriteLine("\t\t<title>BundlePrints</title>");

            #region css
            sw.WriteLine("\t\t<style type=\"text/css\">");
            sw.WriteLine("\t\t\tbody{ margin: 0px; }");
            sw.WriteLine("\t\t\t.page{ page-break-after: always; }");

            // Header
            sw.WriteLine("\t\t\t.header{ height: 100px; }");
            sw.WriteLine("\t\t\timg{ width: 240px; height: 72px; float: left; }");
            sw.WriteLine("\t\t\t.version{ float: right; padding-top: 30px; }");

            // Bundle Info
            sw.WriteLine("\t\t\t.bundle-info{ height: 200px; }");
            sw.WriteLine("\t\t\t.bundle-info-left-side{ float: left; }");
            sw.WriteLine("\t\t\t.bundle-info-right-side{ float: right; }");
            sw.WriteLine("\t\t\t.title{ float: left; }");
            sw.WriteLine("\t\t\t.data{ float: right; }");
            sw.WriteLine("\t\t\tul{ list-style-type: none; padding-left: 0px; padding-right: 50px; }");
            sw.WriteLine("\t\t\tli{ padding-top: 8px; }");

            // Views
            sw.WriteLine("\t\t\t.length-view{ height: 250px; }");
            sw.WriteLine("\t\t\t.width-view{ height: 250px; }");
            sw.WriteLine("\t\t\t.view-title{ text-align: center; padding-bottom: 30px; }");
            sw.WriteLine("\t\t\t.panel{ border: 1px solid black; }");
            sw.WriteLine("\t\t\t.sticker{ height: 6px; }");

            // Footer
            sw.WriteLine("\t\t\t.footer{ padding-top: 200px; }");
            sw.WriteLine("\t\t\t.footer-project{ display: inline-block; width: 310px; float: left; }");
            sw.WriteLine("\t\t\t.footer-date-created{ display: inline-block; width: 100px; }");
            sw.WriteLine("\t\t\t.footer-page-index{ display: inline-block; width: 200px; float: right; text-align: right; }");

            sw.WriteLine("\t\t</style>");
            #endregion

            sw.WriteLine("\t</head>");
            sw.WriteLine("\t<body>");
        }

        private static void CreatePage(Bundle bundle, StreamWriter sw)
        {
            sw.WriteLine("\t\t<div class=\"page\">");

            // Header
            sw.WriteLine(String.Format("\t\t\t<div class=\"header\"><img src=\"{0}\"/><div class=\"version\">BundleBuilder v{1}</div></div>", imageFile, ProjectState.Version));

            // Bundle Info
            sw.WriteLine("\t\t\t<div class=\"bundle-info\">");
            sw.WriteLine("\t\t\t\t<div class=\"bundle-info-left-side\">");
            sw.WriteLine("\t\t\t\t\t<div class=\"title\">");
            sw.WriteLine("\t\t\t\t\t\t<ul>");
            sw.WriteLine("\t\t\t\t\t\t\t<li>Project Name</li>");
            sw.WriteLine("\t\t\t\t\t\t\t<li>Project Number</li>");
            sw.WriteLine("\t\t\t\t\t\t\t<li>Project Location</li>");
            sw.WriteLine("\t\t\t\t\t\t\t<li>Bundle</li>");
            sw.WriteLine("\t\t\t\t\t\t</ul>");
            sw.WriteLine("\t\t\t\t\t</div>");
            sw.WriteLine("\t\t\t\t\t<div class=\"data\">");
            sw.WriteLine("\t\t\t\t\t\t<ul>");
            sw.WriteLine(String.Format("\t\t\t\t\t\t\t<li>{0}</li>", Project.Name));
            sw.WriteLine(String.Format("\t\t\t\t\t\t\t<li>{0}</li>", Project.Number)); 
            sw.WriteLine(String.Format("\t\t\t\t\t\t\t<li>{0}</li>", Project.Location));
            sw.WriteLine(String.Format("\t\t\t\t\t\t\t<li>{0}</li>", bundle.ToString()));
            sw.WriteLine("\t\t\t\t\t\t</ul>");
            sw.WriteLine("\t\t\t\t\t</div>");
            sw.WriteLine("\t\t\t\t</div>");
            sw.WriteLine("\t\t\t\t<div class=\"bundle-info-right-side\">");
            sw.WriteLine("\t\t\t\t\t<div class=\"title\">");
            sw.WriteLine("\t\t\t\t\t\t<ul>");
            sw.WriteLine("\t\t\t\t\t\t\t<li>Bundle Length</li>");
            sw.WriteLine("\t\t\t\t\t\t\t<li>Bundle Width</li>");
            sw.WriteLine("\t\t\t\t\t\t\t<li>Bundle Height</li>");
            sw.WriteLine("\t\t\t\t\t\t\t<li>Bundle Weight</li>");
            sw.WriteLine("\t\t\t\t\t\t</ul>");
            sw.WriteLine("\t\t\t\t\t</div>");
            sw.WriteLine("\t\t\t\t\t<div class=\"data\">");
            sw.WriteLine("\t\t\t\t\t\t<ul>");
            sw.WriteLine(String.Format("\t\t\t\t\t\t\t<li>{0}</li>", Tools.DimensionTools.AsString(bundle.Length))); 
            sw.WriteLine(String.Format("\t\t\t\t\t\t\t<li>{0}</li>", Tools.DimensionTools.AsString(bundle.Width))); 
            sw.WriteLine(String.Format("\t\t\t\t\t\t\t<li>{0}</li>", Tools.DimensionTools.AsString(bundle.Height))); 
            sw.WriteLine(String.Format("\t\t\t\t\t\t\t<li>{0} lbs.</li>", bundle.Weight)); 
            sw.WriteLine("\t\t\t\t\t\t</ul>");
            sw.WriteLine("\t\t\t\t\t</div>");
            sw.WriteLine("\t\t\t\t</div>");
            sw.WriteLine("\t\t\t</div>");

            // Length View
            sw.WriteLine("\t\t\t<div class=\"length-view\">");
            sw.WriteLine("\t\t\t\t<div class=\"view-title\">Length View</div>");
            sw.WriteLine("\t\t\t\t<div class=\"view-bundle\">");

            foreach (Level level in bundle.Levels)
            {
                sw.WriteLine();
                sw.Write(String.Format("\t\t\t\t\t<div class=\"panel\" style=\"width: {0}px; height: {1}px;\">", (level.Length * 2.25) + 6, (level.Height * 2.25) + 6));

                foreach (Panel panel in level.Panels)
                    sw.Write(panel.Name.FullName + " ");

                sw.Write("</div>");
                sw.WriteLine("\t\t\t\t\t<div class=\"sticker\"></div>");
            }

            sw.WriteLine("\t\t\t\t</div>");
            sw.WriteLine("\t\t\t</div>");

            // Width View
            sw.WriteLine("\t\t\t<div class=\"width-view\">");
            sw.WriteLine("\t\t\t\t<div class=\"view-title\">Width View</div>");
            sw.WriteLine("\t\t\t\t<div class=\"view-bundle\">");

            foreach (Level level in bundle.Levels)
            {
                sw.WriteLine(String.Format("\t\t\t\t\t<div class=\"panel\" style=\"width: {0}px; height: {1}px;\"></div>", (level.Width * 2.25) + 6, (level.Height * 2.25) + 6));
                sw.WriteLine("\t\t\t\t\t<div class=\"sticker\"></div>");
            }

            sw.WriteLine("\t\t\t\t</div>");
            sw.WriteLine("\t\t\t</div>");

            // Footer
            sw.WriteLine("\t\t\t<div class=\"footer\">");
            sw.WriteLine(String.Format("\t\t\t\t<div class=\"footer-project\">{0}</div>", Project.Number));
            sw.WriteLine(String.Format("\t\t\t\t<div class=\"footer-date-created\">{0}</div>", DateTime.Now.ToShortDateString()));
            sw.WriteLine(String.Format("\t\t\t\t<div class=\"footer-page-index\">Bundle {0}/{1}</div>", bundle.Number, Project.Bundles.Count));
            sw.WriteLine("\t\t\t</div>");

            sw.WriteLine("\t\t</div>");
        }

        private static void CreateFileFooter(StreamWriter sw)
        {
            sw.WriteLine("\t</body>");
            sw.WriteLine("</html>");
        }
    }
}
