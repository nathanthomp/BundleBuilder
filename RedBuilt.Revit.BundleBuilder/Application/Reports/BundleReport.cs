using RedBuilt.Revit.BundleBuilder.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.Application.Reports
{
    public class BundleReport
    {
        private static readonly string filePath = @"C:\ProgramData\RedBuilt\RBBundleBuilder\BundleReport.html";

        /// <summary>
        /// Creates the html document
        /// </summary>
        public static void CreateHtml()
        {
            StreamWriter sw = new StreamWriter(filePath);

            CreateFileHeader(sw);

            // CreateTable(sw);

            foreach (Bundle bundle in Project.Bundles)
            {
                CreateBundle(bundle, sw);
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
            sw.WriteLine("\t\t<meta http-equiv=\"X-UA-Compatible\" content=\"IE=9\"/>");
            sw.WriteLine("\t\t<title>Bundles</title>");

            #region css
            sw.WriteLine("\t\t<style type=\"text/css\">");

            sw.WriteLine("\t\t\t.bundle{ margin-bottom: 60px; }");

            // Header
            sw.WriteLine("\t\t\t.bundle-header{ height: 20px; }");
            sw.WriteLine("\t\t\t.bundle-header-title{ float: left; }");
            sw.WriteLine("\t\t\t.bundle-header-info{ float: right; }");

            // Views
            sw.WriteLine("\t\t\t.bundle-view{ overflow: auto; }");
            sw.WriteLine("\t\t\t.bundle-view-title{ text-align: center; margin-bottom: 10px; margin-top: 10px; }");
            sw.WriteLine("\t\t\t.bundle-view-length{ overflow: auto; }");
            sw.WriteLine("\t\t\t.bundle-view-length-display{ float: left; }");
            sw.WriteLine("\t\t\t.bundle-view-length-captions{ float: right; height: 25px; /* height + 13 */ margin-right: 10px; }");
            sw.WriteLine("\t\t\t.bundle-view-width-top{ width: 40%; float: right; }");
            sw.WriteLine("\t\t\t.bundle-view-width-bottom{ text-align: right; width: 40%; float: left; }");
            sw.WriteLine("\t\t\t.bundle-view-subtitle{ height: 20px; padding-bottom: 60px; }");
            sw.WriteLine("\t\t\t.bundle-view-subtitle-bottom{ width: 50%; text-align: center; float: left; }");
            sw.WriteLine("\t\t\t.bundle-view-subtitle-top{ width: 50%; float: right; text-align: center; }");
            sw.WriteLine("\t\t\t.level{ display: inline-block; }");
            sw.WriteLine("\t\t\t.depth{ display: inline-block; }");
            sw.WriteLine("\t\t\t.column{ display: inline-block; }");
            sw.WriteLine("\t\t\t.panel{ border: 1px solid black; font-size: 10px; }");
            sw.WriteLine("\t\t\t.sticker{ height: 1px; }");

            sw.WriteLine("\t\t</style>");
            #endregion

            sw.WriteLine("\t</head>");
            sw.WriteLine("\t<body>");
        }

        /// <summary>
        /// Creates a single bundle on the page
        /// </summary>
        /// <param name="bundle">bundle to display</param>
        /// <param name="sw">output stream</param>
        private static void CreateBundle(Bundle bundle, StreamWriter sw)
        {
            sw.WriteLine("\t\t<div class=\"bundle\">");
            sw.WriteLine("\t\t\t<div class=\"bundle-header\">");
            sw.WriteLine(String.Format("\t\t\t\t<div class=\"bundle-header-title\">{0}</div>", bundle.ToString()));
            // sw.WriteLine(String.Format("\t\t\t\t<div class=\"bundle-header-info\">{0} - {1}</div>", bundle.Type, bundle.Plate));
            sw.WriteLine("\t\t\t</div>");

            sw.WriteLine("\t\t\t<div class=\"bundle-view\">");
            sw.WriteLine("\t\t\t\t<div class=\"bundle-view-length\">");
            sw.WriteLine("\t\t\t\t\t<div class=\"bundle-view-title\">Length View</div>");
            sw.WriteLine("\t\t\t\t\t<div class=\"bundle-view-length-display\">");

            CreateLengthLevels(bundle, sw);

            sw.WriteLine("\t\t\t\t\t</div>"); // bundle view length display
            sw.WriteLine("\t\t\t\t\t<div class=\"bundle-view-length-captions\">");

            for (int i = bundle.Levels.Count; i > 0; i--)
                sw.WriteLine(String.Format("\t\t\t\t\t\t<div style=\" height: {0}px; \">Level {1}</div>", bundle.Levels.First().Height * 2 + 8, bundle.Levels.Where(x => x.Number == i).First().Number));

            sw.WriteLine("\t\t\t\t\t</div>"); // bundle view length captions
            sw.WriteLine("\t\t\t\t</div>"); // bundle view length
            sw.WriteLine("\t\t\t\t<div class=\"bundle-view-width\">");
            sw.WriteLine("\t\t\t\t\t<div class=\"bundle-view-title\">Width View</div>");

            CreateWidthLevels(bundle, sw);

            sw.WriteLine("\t\t\t\t</div>"); // bundle view width
            sw.WriteLine("\t\t\t</div>"); // bundle view
            sw.WriteLine("\t\t\t<div class=\"bundle-view-subtitle\">");
            sw.WriteLine("\t\t\t\t<div class=\"bundle-view-subtitle-bottom\">Bottom View</div>");
            sw.WriteLine("\t\t\t\t<div class=\"bundle-view-subtitle-top\">Top View</div>");
            sw.WriteLine("\t\t\t</div>"); // subtitle
            sw.WriteLine("\t\t</div>"); // bundle

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
                sw.WriteLine("\t\t\t\t\t\t<div class=\"level\">");

                // Get number of depths
                int numOfDepths = level.Panels.Max(x => x.Depth);
                for (int j = 0; j < numOfDepths; j++)
                {
                    // Get depth length
                    double maxLengthOfDepth = level.Panels.Where(x => x.Depth == j + 1).Max(x => x.Height.AsDouble);
                    double depthLength = maxLengthOfDepth * 2;
                    if (maxDepthsInBundle > 1 && numOfDepths == 1)
                        depthLength += (maxDepthsInBundle - 1) * 6;

                    // Get panel names
                    string panelNames = "";
                    //foreach (Panel panel in level.Panels.Where(x => x.Depth == j + 1))
                    //    panelNames += panel.Name.FullName + " ";

                    // Create depth
                    sw.WriteLine(String.Format("\t\t\t\t\t\t\t<div class=\"depth panel\" style=\"width: {0}px; height: {1}px;\">{2}</div>", depthLength, (level.Height * 2) + 6, panelNames));
                }
                sw.WriteLine("\t\t\t\t\t\t</div>");
                sw.WriteLine("\t\t\t\t\t\t<div class=\"sticker\"></div>");
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
            sw.WriteLine("\t\t\t\t\t<div class=\"bundle-view-width-bottom\">");
            for (int i = bundle.Levels.Count; i > 0; i--)
            {
                Level level = bundle.Levels.Where(x => x.Number == i).First();
                sw.WriteLine("\t\t\t\t\t\t<div class=\"level\">");

                // Find number of columns in level where depth is 1
                int numOfColumns = level.Panels.Where(x => x.Depth == 1).Max(x => x.Column);

                // Print all of the columns with depth of 1
                for (int j = numOfColumns; j > 0; j--)
                {
                    Panel panel = level.Panels.Where(x => x.Depth == 1 && x.Column == j).First();

                    // Get column width
                    double columnWidth = panel.Width.AsDouble * 2;
                    if (maxColumnsInBundle > 1 && numOfColumns == 1)
                        columnWidth += (maxColumnsInBundle - 1) * 6;

                    sw.WriteLine(String.Format("\t\t\t\t\t\t\t<div class=\"column panel\" style=\"width: {0}px; height: {1}px;\">{2}</div>", columnWidth, (level.Height * 2) + 6, panel.Name.FullName));
                }

                sw.WriteLine("\t\t\t\t\t\t</div>");
                sw.WriteLine("\t\t\t\t\t\t<div class=\"sticker\"></div>");

            }
            sw.WriteLine("\t\t\t\t\t</div>"); 

            // Top Side
            sw.WriteLine("\t\t\t\t\t<div class=\"bundle-view-width-top\">");
            for (int i = bundle.Levels.Count; i > 0; i--)
            {
                Level level = bundle.Levels.Where(x => x.Number == i).First();
                sw.WriteLine("\t\t\t\t\t\t<div class=\"level column\">");

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
                        double columnWidth = panel.Width.AsDouble * 2;
                        if (maxColumnsInBundle > 1 && numOfColumns == 1)
                            columnWidth += (maxColumnsInBundle - 1) * 6;

                        // Create column
                        sw.WriteLine(String.Format("\t\t\t\t\t\t\t<div class=\"column panel\" style=\"width: {0}px; height: {1}px;\">{2}</div>", columnWidth, (level.Height * 2) + 6, panel.Name.FullName));
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
                        double columnWidth = panel.Width.AsDouble * 2;
                        if (maxColumnsInBundle > 1 && numOfColumns == 1)
                            columnWidth += (maxColumnsInBundle - 1) * 6;

                        // Create column
                        sw.WriteLine(String.Format("\t\t\t\t\t\t\t<div class=\"column panel\" style=\"width: {0}px; height: {1}px;\">{2}</div>", columnWidth, (level.Height * 2) + 6, panel.Name.FullName));
                    }
                }

                sw.WriteLine("\t\t\t\t\t\t</div>");
                sw.WriteLine("\t\t\t\t\t\t<div class=\"sticker\"></div>");

            }
            sw.WriteLine("\t\t\t\t\t</div>");
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
