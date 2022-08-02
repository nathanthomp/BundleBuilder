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

        /// <summary>
        /// Creates the html document and converts it to a pdf document
        /// </summary>
        /// <param name="fileName">the pdf document file path</param>
        /// <exception cref="Exception">I/O problems</exception>
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

        /// <summary>
        /// Creates the html document
        /// </summary>
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

        /// <summary>
        /// Creates the static header for the html document including the 
        /// doc type, html, head, and initial body tags
        /// </summary>
        /// <param name="sw">output stream</param>
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
            sw.WriteLine("\t\t\t.additional-info{ font-size: 14px; padding-top: 2px; }");
            sw.WriteLine("\t\t\t.sticker-info{ float: right; padding-right: 50px; }");

            // Views
            sw.WriteLine("\t\t\t.length-view{ height: 250px; }");
            sw.WriteLine("\t\t\t.width-view{ height: 250px; }");
            sw.WriteLine("\t\t\t.view-title{ text-align: center; padding-bottom: 30px; }");
            sw.WriteLine("\t\t\t.view-bundle-bottom{ float: left; width: 40%; text-align: right; }");
            sw.WriteLine("\t\t\t.view-bundle-top{ float: right; width: 40%; }");
            sw.WriteLine("\t\t\t.level{ display: inline-block; }");
            sw.WriteLine("\t\t\t.depth{ display: inline-block; }");
            sw.WriteLine("\t\t\t.column{ display: inline-block; }");
            sw.WriteLine("\t\t\t.panel{ border: 1px solid black; font-size: 10px; }");
            sw.WriteLine("\t\t\t.panel-ghost{ border: 1px solid dashed; }");
            sw.WriteLine("\t\t\t.sticker{ height: 6px; }");
            sw.WriteLine("\t\t\t.width-subtitle-left{ float: left; margin-top: 20px; width: 50%; text-align: center; }");
            sw.WriteLine("\t\t\t.width-subtitle-right{ float: right; margin-top: 20px; width: 50%; text-align: center; }");

            // Comment
            sw.WriteLine("\t\t\t.comment{ padding-top: 140px; padding-left: 110px }");

            // Footer
            sw.WriteLine("\t\t\t.footer{ padding-top: 50px; }");
            sw.WriteLine("\t\t\t.footer-project{ display: inline-block; width: 310px; float: left; }");
            sw.WriteLine("\t\t\t.footer-date-created{ display: inline-block; width: 100px; }");
            sw.WriteLine("\t\t\t.footer-page-index{ display: inline-block; width: 200px; float: right; text-align: right; }");

            // Data Page
            sw.WriteLine("\t\t\ttable{ margin-left: auto; margin-right: auto; border-collapse: collapse; text-align: center; padding-top: 100px; }");
            sw.WriteLine("\t\t\tth{ padding-left: 40px; padding-right: 40px; padding-top: 100px; padding-bottom: 8px; }");
            sw.WriteLine("\t\t\ttr{ border-bottom: 1px solid black; padding: 8px; }");
            sw.WriteLine("\t\t\ttd{ padding-top: 4px; padding-bottom: 4px; }");
            
            sw.WriteLine("\t\t</style>");
            #endregion

            sw.WriteLine("\t</head>");
            sw.WriteLine("\t<body>");
        }

        /// <summary>
        /// Creates a single html/pdf page for a single bundle
        /// </summary>
        /// <param name="bundle">bundle to display on page</param>
        /// <param name="sw">output stream</param>
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
            sw.WriteLine("\t\t\t\t\t\t\t<li>Bundle Weight</li>");
            sw.WriteLine("\t\t\t\t\t\t\t<li>Bundle Height</li>");
            sw.WriteLine("\t\t\t\t\t\t</ul>");
            sw.WriteLine("\t\t\t\t\t</div>");
            sw.WriteLine("\t\t\t\t\t<div class=\"data\">");
            sw.WriteLine("\t\t\t\t\t\t<ul>");
            sw.WriteLine(String.Format("\t\t\t\t\t\t\t<li>{0}</li>", Tools.DimensionTools.AsString(bundle.Length))); 
            sw.WriteLine(String.Format("\t\t\t\t\t\t\t<li>{0}</li>", Tools.DimensionTools.AsString(bundle.Width))); 
            sw.WriteLine(String.Format("\t\t\t\t\t\t\t<li>{0} lbs.</li>", bundle.Weight));
            sw.WriteLine(String.Format("\t\t\t\t\t\t\t<li>{0}</li>", Tools.DimensionTools.AsString(bundle.Height)));
            sw.WriteLine("\t\t\t\t\t\t</ul>");
            sw.WriteLine("\t\t\t\t\t</div>");
            sw.WriteLine("\t\t\t\t</div>");
            sw.WriteLine("\t\t\t\t<div class=\"additional-info\">");
            sw.WriteLine("\t\t\t\t\t<div class=\"sticker-info\">* Height assumes 3\" stickers</div>");
            sw.WriteLine("\t\t\t\t</div>");
            sw.WriteLine("\t\t\t</div>");

            // Length View
            sw.WriteLine("\t\t\t<div class=\"length-view\">");
            sw.WriteLine(String.Format("\t\t\t\t<div class=\"view-title\">Length View - Allowable Overhang {0}%</div>", Settings.LengthMargin * 100));
            sw.WriteLine("\t\t\t\t<div class=\"view-bundle\">");

            CreateLengthLevels(bundle, sw);

            sw.WriteLine("\t\t\t\t</div>");
            sw.WriteLine("\t\t\t</div>");

            // Width View
            sw.WriteLine("\t\t\t<div class=\"width-view\">");
            sw.WriteLine(String.Format("\t\t\t\t<div class=\"view-title\">Width View - Allowable Overhang {0}%</div>", Settings.WidthMargin * 100));
            
            CreateWidthLevels(bundle, sw);

            sw.WriteLine("\t\t\t\t<div>");
            sw.WriteLine("\t\t\t\t\t<div class=\"width-subtitle-right\">Top View</div>");
            sw.WriteLine("\t\t\t\t\t<div class=\"width-subtitle-left\">Bottom View</div>");
            sw.WriteLine("\t\t\t\t</div>");

            sw.WriteLine("\t\t\t</div>");

            // Comment
            sw.WriteLine("\t\t\t<div class=\"comment\">");
            sw.WriteLine("\t\t\t\tComment: _________________________________________________");
            sw.WriteLine("\t\t\t</div>");

            // Footer
            sw.WriteLine("\t\t\t<div class=\"footer\">");
            sw.WriteLine(String.Format("\t\t\t\t<div class=\"footer-project\">{0}</div>", Project.Number));
            sw.WriteLine(String.Format("\t\t\t\t<div class=\"footer-date-created\">{0}</div>", DateTime.Now.ToShortDateString()));
            sw.WriteLine(String.Format("\t\t\t\t<div class=\"footer-page-index\">Bundle {0}/{1}</div>", bundle.Number, Project.Bundles.Count));
            sw.WriteLine("\t\t\t</div>");

            sw.WriteLine("\t\t</div>");

            // Data Page
            sw.WriteLine("\t\t<div class=\"page\">");
            sw.WriteLine("\t\t\t<table>");
            sw.WriteLine("\t\t\t\t<tr>");
            sw.WriteLine("\t\t\t\t\t<th>Panel</th><th>Level</th><th>Height</th><th>Width</th><th>Weight</th>");
            sw.WriteLine("\t\t\t\t</tr>");

            for (int i = bundle.Levels.Count; i > 0; i--)
            {
                Level level = bundle.Levels.Where(x => x.Number == i).First();

                foreach (Panel panel in level.Panels)
                {
                    sw.WriteLine("\t\t\t\t<tr>");
                    sw.WriteLine(String.Format("\t\t\t\t\t<td>{0}</td>", panel.Name.FullName));
                    sw.WriteLine(String.Format("\t\t\t\t\t<td>{0}</td>", panel.Level.Number));
                    sw.WriteLine(String.Format("\t\t\t\t\t<td>{0}</td>", panel.Height.AsString));
                    sw.WriteLine(String.Format("\t\t\t\t\t<td>{0}</td>", panel.Width.AsString));
                    sw.WriteLine(String.Format("\t\t\t\t\t<td>{0} lbs.</td>", panel.Weight));
                    sw.WriteLine("\t\t\t\t</tr>");
                }
            }

            sw.WriteLine("\t\t\t</table>");
            sw.WriteLine("\t\t</div>");
        }

        /// <summary>
        /// Creates a length view for a single bundle
        /// </summary>
        /// <param name="bundle">bundle length view to display</param>
        /// <param name="sw">output stream</param>
        private static void CreateLengthLevels(Bundle bundle, StreamWriter sw)
        {
            // Get maximum number of depths in the bundle
            List<int> depths = new List<int>();
            bundle.Levels.ForEach(level => level.Panels.ForEach(panel => depths.Add(panel.Depth)));
            int maxDepthsInBundle = depths.Max();

            // Create each level
            for (int i = bundle.Levels.Count; i > 0; i--)
            {
                Level level = bundle.Levels.Where(x => x.Number == i).First();

                sw.WriteLine();
                sw.WriteLine("\t\t\t\t\t<div class=\"level\">");

                // Get number of depths
                int numOfDepths = level.Panels.Max(x => x.Depth);
                for (int j = 0; j < numOfDepths; j++)
                {
                    // Get depth length
                    double maxLengthOfDepth = level.Panels.Where(x => x.Depth == j + 1).Max(x => x.Height.AsDouble);
                    double depthLength = maxLengthOfDepth * 2.25;
                    if (maxDepthsInBundle > 1 && numOfDepths == 1)
                        depthLength += (maxDepthsInBundle - 1) * 6;

                    // Get panel names
                    string panelNames = "";
                    //foreach (Panel panel in level.Panels.Where(x => x.Depth == j + 1))
                    //    panelNames += panel.Name.FullName + " ";

                    // Create depth
                    sw.WriteLine(String.Format("\t\t\t\t\t\t<div class=\"depth panel\" style=\"width: {0}px; height: {1}px;\">{2}</div>", depthLength, (level.Height * 2.25) + 6, panelNames));
                }
                sw.WriteLine("\t\t\t\t\t</div>");
                sw.WriteLine("\t\t\t\t\t<div class=\"sticker\"></div>");
            }
        }

        /// <summary>
        /// Creates a width view for a single bundle
        /// </summary>
        /// <param name="bundle">bundle width view to display</param>
        /// <param name="sw">output stream</param>
        private static void CreateWidthLevels(Bundle bundle, StreamWriter sw)
        {
            // Get maximum number of columns in the bundle
            List<int> columns = new List<int>();
            bundle.Levels.ForEach(level => level.Panels.ForEach(panel => columns.Add(panel.Column)));
            int maxColumnsInBundle = columns.Max();


            // Bottom Side
            sw.WriteLine("\t\t\t\t<div class=\"view-bundle-bottom\">");
            for (int i = bundle.Levels.Count; i > 0; i--)
            {
                Level level = bundle.Levels.Where(x => x.Number == i).First();
                sw.WriteLine("\t\t\t\t\t<div class=\"level\">");

                // Find number of columns in level where depth is 1
                int numOfColumns = level.Panels.Where(x => x.Depth == 1).Max(x => x.Column);
 
                // Print all of the columns with depth of 1
                for (int j = numOfColumns; j > 0; j--)
                {
                    Panel panel = level.Panels.Where(x => x.Depth == 1 && x.Column == j).First();

                    // Get column width
                    double columnWidth = panel.Width.AsDouble * 2.25;
                    if (maxColumnsInBundle > 1 && numOfColumns == 1)
                        columnWidth += (maxColumnsInBundle - 1) * 6;

                    sw.WriteLine(String.Format("\t\t\t\t\t\t<div class=\"column panel\" style=\"width: {0}px; height: {1}px;\">{2}</div>", columnWidth, (level.Height * 2.25) + 6, panel.Name.FullName));
                }

                sw.WriteLine("\t\t\t\t\t\t</div>");
                sw.WriteLine("\t\t\t\t\t\t<div class=\"sticker\"></div>");
                
            }
            sw.WriteLine("\t\t\t\t</div>");

            // Top Side
            sw.WriteLine("\t\t\t\t<div class=\"view-bundle-top\">");
            for (int i = bundle.Levels.Count; i > 0; i--)
            {
                Level level = bundle.Levels.Where(x => x.Number == i).First();
                sw.WriteLine("\t\t\t\t\t<div class=\"level column\">");

                List<Panel> topViewPanels = level.Panels.Where(x => x.Depth == 2).ToList();
                
                if (topViewPanels.Count == 0)
                {
                    // Find number of columns in level where depth is 1
                    int numOfColumns = level.Panels.Where(x => x.Depth == 1).Max(x => x.Column);

                    // Print all of the columns with a depth of 1
                    for (int j = 0; j < numOfColumns; j++)
                    {
                        Panel panel = level.Panels.Where(x => x.Depth == 1 && x.Column == j + 1).First();

                        // Get column width
                        double columnWidth = panel.Width.AsDouble * 2.25;
                        if (maxColumnsInBundle > 1 && numOfColumns == 1)
                            columnWidth += (maxColumnsInBundle - 1) * 6;

                        // Create column
                        sw.WriteLine(String.Format("\t\t\t\t\t\t<div class=\"column panel\" style=\"width: {0}px; height: {1}px;\">{2}</div>", columnWidth, (level.Height * 2.25) + 6, panel.Name.FullName));
                    }
                }
                else
                {
                    // Find number of columns in level where depth is 2
                    int numOfColumns = level.Panels.Where(x => x.Depth == 2).Max(x => x.Column);


                    // Print all of the columns with a depth of 2
                    for (int j = 0; j < numOfColumns; j++)
                    {
                        Panel panel = level.Panels.Where(x => x.Depth == 2 && x.Column == j + 1).First();

                        // Get column width
                        double columnWidth = panel.Width.AsDouble * 2.25;
                        if (maxColumnsInBundle > 1 && numOfColumns == 1)
                            columnWidth += (maxColumnsInBundle - 1) * 6;

                        // Create column
                        sw.WriteLine(String.Format("\t\t\t\t\t\t<div class=\"column panel\" style=\"width: {0}px; height: {1}px;\">{2}</div>", columnWidth, (level.Height * 2.25) + 6, panel.Name.FullName));
                    }
                }

                sw.WriteLine("\t\t\t\t\t\t</div>");
                sw.WriteLine("\t\t\t\t\t\t<div class=\"sticker\"></div>");

            }
            sw.WriteLine("\t\t\t\t\t\t</div>");
        }

        /// <summary>
        /// Creates static footer for the html document including
        /// closing body, and html tags
        /// </summary>
        /// <param name="sw"></param>
        private static void CreateFileFooter(StreamWriter sw)
        {
            sw.WriteLine("\t</body>");
            sw.WriteLine("</html>");
        }
    }
}
