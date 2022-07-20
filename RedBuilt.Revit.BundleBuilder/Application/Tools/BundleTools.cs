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

        public static void CorrectLevelNumbers()
        {
            // Increment through all of the bundles in Project.Bundles
            for (int i = 0; i < Project.Bundles.Count; i++)
            {
                // Select a bundle
                Bundle bundle = Project.Bundles[i];
                List<Level> levelsCopy = new List<Level>(bundle.Levels);

                // Increment for number of levels
                for (int j = bundle.Levels.Count; j > 0; j--)
                {
                    // Get level with the largest number
                    Level level = LevelTools.GetLevelFromNumberAndBundle(bundle, levelsCopy.Max(x => x.Number));

                    // Remove the largest level from levelsCopy
                    levelsCopy.Remove(level);

                    // Change level number to the levels.count
                    level.Number = j;
                }
            }
        }

        public static void CorrectLevelNumbers(Bundle bundle)
        {
            List<Level> levelsCopy = new List<Level>(bundle.Levels);

            // Increment for number of levels
            for (int j = bundle.Levels.Count; j > 0; j--)
            {
                // Get level with the largest number
                Level level = LevelTools.GetLevelFromNumberAndBundle(bundle, levelsCopy.Max(x => x.Number));

                // Remove the largest level from levelsCopy
                levelsCopy.Remove(level);

                // Change level number to the levels.count
                level.Number = j;
            }
        }

    }
}
