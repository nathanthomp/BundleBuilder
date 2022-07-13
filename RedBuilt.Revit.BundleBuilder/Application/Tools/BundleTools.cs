using RedBuilt.Revit.BundleBuilder.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.Application.Tools
{
    public class BundleTools
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static double[] CreateWidthBounds(Level level)
        {
            double[] result = new double[2];
            double width = level.Width;
            result[0] = width - (width * Settings.WidthMargin);
            result[1] = width + (width * Settings.WidthMargin);
            if (result[1] > Settings.MaxBundleWidth)
            {
                result[1] = Settings.MaxBundleWidth;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static double[] CreateLengthBounds(Level level)
        {
            double[] result = new double[2];
            double length = level.Length;
            result[0] = length - (length * Settings.LengthMargin);
            result[1] = length + (length * Settings.LengthMargin);
            if (result[1] > Settings.MaxBundleLength)
            {
                result[1] = Settings.MaxBundleLength;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="widthBounds"></param>
        /// <param name="lengthBounds"></param>
        /// <param name="panelList"></param>
        /// <returns></returns>
        public static int FindIndex(double[] widthBounds, double[] lengthBounds, List<Panel> panelList)
        {
            int index = -1, count = 0;
            foreach (Panel panel in panelList)
            {
                if (panel.IsWithinBounds(widthBounds, lengthBounds))
                {
                    index = count;
                }
                count++;
            }
            return index;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plate"></param>
        /// <returns></returns>
        public static int GetNumberOfLevels(string type, string plate)
        {
            if (type.Equals("Parapet"))
            {
                return 6;
            }
            else
            {
                switch (plate)
                {
                    case "2x4":
                        return 6;
                    case "400":
                        return 6;
                    case "2x6":
                        return 4;
                    case "550":
                        return 4;
                    case "600":
                        return 4;
                    case "2x8":
                        return 3;
                    case "800":
                        return 3;
                    case "2x10":
                        return 2;
                    case "1000":
                        return 2;
                    case "2x12":
                        return 2;
                    case "1200":
                        return 2;
                    default:
                        return 1;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bundle"></param>
        /// <returns></returns>
        public static int NumberOfEmptyLevels(Bundle bundle)
        {
            int result = 0;
            foreach (Level level in bundle.Levels)
            {
                if (level.Panels.Count == 0)
                {
                    result++;
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CorrectBundleLevelNumbers()
        {
            for (int i = 0; i < Project.Bundles.Count; i++)
            {
                Bundle bundle = Project.Bundles[i];

                int counter = 1;
                for (int j = bundle.NumberOfLevels - 1; j >= 0; j--)
                {
                    bundle.Levels[j].Number = counter;
                    counter++;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CorrectBundleNumbers()
        {
            int counter = 1;
            for (int i = 0; i < Project.Bundles.Count; i++)
            {
                Bundle bundle = Project.Bundles[i];
                bundle.Number = counter;
                counter++;
            }
        }
    }
}
