using RedBuilt.Revit.BundleBuilder.Application.Tools;
using RedBuilt.Revit.BundleBuilder.Data.Models;
using RedBuilt.Revit.BundleBuilder.Data.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.Application.Solve
{
    public class BundleSolve
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="plate"></param>
        /// <param name="panelList"></param>
        public static void Solve(string type, string plate, List<Panel> panelList)
        {
            List<Panel> panelListCopy = new List<Panel>(panelList);
            int numberOfLevels = BundleTools.GetNumberOfLevels(plate);

            while (panelListCopy.Count > 0)
            {
                // Establish Bundle
                Bundle bundle = new Bundle(Project.CurrentBundleNumber, numberOfLevels)
                {
                    Type = type,
                    Plate = plate
                };
                double[] bundleWidthBounds = new double[2];
                double[] bundleLengthBounds = new double[2];

                // Fill in Bundle with Levels
                for (int i = numberOfLevels; i > 0; i--)
                {
                    Level level = new Level(i);
                    bundle.Add(level);
                    level.Bundle = bundle;
                }

                // For each level in the Bundle, look at each Panel in List to see if it will fit within the Level
                // then remove the Panel from the List if it does fit. Does not look for another Panel if the List
                // is empty.
                for (int i = 0; i < numberOfLevels; i++)
                {

                    // top level, squeeze as many panels in as possible
                    if (i == 0)
                    {
                        Level level = bundle.Levels[i];
                        for (int j = 0; j < panelListCopy.Count; j++)
                        {
                            if (panelListCopy.Count != 0)
                            {
                                Panel panel = panelListCopy[j];

                                // if level has less than MaxPanelsPerLevel
                                if (level.Panels.Count < Settings.MaxPanelsPerLevel)
                                {
                                    if ((panel.Width.AsDouble + level.Width) < Settings.MaxBundleWidth)
                                    {
                                        level.Add(panel);
                                        panelListCopy.RemoveAt(j);
                                    }
                                }
                            }
                        }
                        bundleWidthBounds = BundleTools.CreateWidthBounds(level);
                        bundleLengthBounds = BundleTools.CreateLengthBounds(level);
                    }

                    // intermediate level, place as many panels as possible within bounds
                    else if (i < numberOfLevels - 1)
                    {
                        Level level = bundle.Levels[i];
                        for (int j = 0; j < panelListCopy.Count; j++)
                        {
                            if (panelListCopy.Count != 0)
                            {
                                Panel panel = panelListCopy[j];

                                if (level.Panels.Count < Settings.MaxPanelsPerLevel)
                                {
                                    // if adding to this level will make the level width and length less than the width and length upperbound
                                    if ((panel.Width.AsDouble + level.Width) < bundleWidthBounds[1]
                                        &&
                                        (panel.Width.AsDouble + level.Width) > bundleWidthBounds[0])
                                    {
                                        if ((panel.Height.AsDouble + level.Length) > bundleLengthBounds[0]
                                            &&
                                            (panel.Height.AsDouble + level.Length) < bundleLengthBounds[1])
                                        {
                                            level.Add(panel);
                                            panelListCopy.RemoveAt(j);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // bottom level, place single panel that is big enough
                    else if (i == numberOfLevels - 1)
                    {
                        Level level = bundle.Levels[i];
                        for (int j = 0; j < panelListCopy.Count; j++)
                        {
                            if (panelListCopy.Count != 0)
                            {
                                Panel panel = panelListCopy[j];

                                if (panel.IsWithinBounds(bundleWidthBounds, bundleLengthBounds))
                                {
                                    level.Add(panel);
                                    panelListCopy.RemoveAt(j);
                                    break;
                                }
                            }
                        }
                    }

                } // bundle is complete

                // bundle has empty levels
                if (BundleTools.NumberOfEmptyLevels(bundle) > 0)
                {
                    // if there are still panels to bundle
                    if (panelListCopy.Count != 0)
                    {
                         // bundle.Reversed = true;

                        // remove all panels in bundle
                        for (int i = 0; i < bundle.Levels.Count; i++)
                        {
                            Level level = bundle.Levels[i];
                            for (int j = 0; j < level.Panels.Count; j++)
                            {
                                Panel panel = level.Panels[j];
                                level.Remove(panel);
                                panelListCopy.Add(panel);
                            }
                        }
                        // correct the order of panels in panel list
                        // panelListCopy = panelListCopy.OrderBy(x => x.Description).ThenBy(x => x.Instance).ThenBy(x => x.Name).ThenBy(x => x.Variable).ToList();
                        panelListCopy = Sort.PanelNameSort.Sort(panelListCopy); // TODO

                        // pick largest panel remaining and remove it
                        Panel maxPanel = panelListCopy.OrderByDescending(x => x.Area).First();
                        panelListCopy.Remove(maxPanel);

                        // place largest panel at the bottom of the bundle
                        bundle.Levels[numberOfLevels - 1].Add(maxPanel);

                        // update bounds
                        bundleWidthBounds = BundleTools.CreateWidthBounds(bundle.Levels[numberOfLevels - 1]);
                        bundleLengthBounds = BundleTools.CreateLengthBounds(bundle.Levels[numberOfLevels - 1]);

                        // bundle starting at level above bottom level and in reverse order of the panel list
                        for (int i = numberOfLevels - 2; i >= 0; i--)
                        {
                            Level level = bundle.Levels[i];
                            for (int j = panelListCopy.Count - 1; j >= 0; j--)
                            {
                                if (panelListCopy.Count != 0)
                                {
                                    Panel panel = panelListCopy[j];

                                    if (level.Panels.Count < Settings.MaxPanelsPerLevel)
                                    {
                                        if ((panel.Width.AsDouble + level.Width) < bundleWidthBounds[1] && panel.Height.AsDouble < bundleLengthBounds[1])
                                        {
                                            level.Add(panel);
                                            panelListCopy.RemoveAt(j);
                                        }
                                    }
                                }
                            }
                            // update bounds for the top most level
                            bundleWidthBounds = BundleTools.CreateWidthBounds(level);
                            bundleLengthBounds = BundleTools.CreateLengthBounds(level);
                        }
                    }
                }
                Project.Bundles.Add(bundle);
                Project.CurrentBundleNumber++;
            }
        }
    }
}
