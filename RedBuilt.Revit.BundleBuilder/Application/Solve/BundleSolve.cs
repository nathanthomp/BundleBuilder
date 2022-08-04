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
        /// Creates bundles based on the type and plate
        /// 
        /// TODO: extensive documentation
        /// 
        /// 
        /// </summary>
        /// <param name="type">bundle type</param>
        /// <param name="plate">bundle plate</param>
        /// <param name="panelList">list of panels to bundle together</param>
        public static void Solve(string type, string plate, List<Panel> panelList)
        {
            List<Panel> panelListCopy = new List<Panel>(panelList);
            int numberOfLevels = BundleTools.GetNumberOfLevels(type, plate);

            while (panelListCopy.Count > 0)
            {
                #region Establish Bundle

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

                #endregion

                #region Bundle Top Down

                // For each level in the Bundle, look at each Panel in List to see if it will fit within the Level
                // then remove the Panel from the List if it does fit. Does not look for another Panel if the List
                // is empty.
                for (int i = 0; i < numberOfLevels; i++)
                {
                    int colIndex = 1;

                    // top level, squeeze as many panels in as possible
                    if (i == 0)
                    {
                        Level level = bundle.Levels[i];
                        colIndex = 1;
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
                                        if (level.Add(panel))
                                        {
                                            panelListCopy.RemoveAt(j);
                                            panel.Depth = 1;
                                            panel.Column = colIndex;
                                            colIndex++;
                                        }
                                        else
                                            throw new Exception("Cannot add panel to level");
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
                        colIndex = 1;
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
                                            if (level.Add(panel))
                                            {
                                                panelListCopy.RemoveAt(j);
                                                panel.Depth = 1;
                                                panel.Column = colIndex;
                                                colIndex++;
                                            }
                                            else
                                                throw new Exception("Cannot add panel to level");
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
                                    if (level.Add(panel))
                                    {
                                        panelListCopy.RemoveAt(j);
                                        panel.Depth = 1;
                                        panel.Column = 1;
                                    }
                                    else
                                        throw new Exception("Cannot add panel to level");

                                    break;
                                }
                            }
                        }
                    }

                } // bundle is complete

                #endregion

                #region Bundle Bottom Up

                // bundle has empty levels
                if (BundleTools.NumberOfEmptyLevels(bundle) > 0)
                {
                    // if there are still panels to bundle
                    if (panelListCopy.Count != 0)
                    {
                        int colIndex = 1;

                        // remove all panels in bundle
                        for (int i = 0; i < bundle.Levels.Count; i++)
                        {
                            Level level = bundle.Levels[i];
                            for (int j = 0; j < level.Panels.Count; j++)
                            {
                                Panel panel = level.Panels[j];

                                if (level.Remove(panel))
                                    panelListCopy.Add(panel);
                                else
                                    throw new Exception("Could not remove panel from level");
                            }
                        }
                        // correct the order of panels in panel list
                        panelListCopy = Sort.PanelNameSort.Sort(panelListCopy);

                        // pick largest panel remaining and remove it
                        Panel maxPanel = panelListCopy.OrderByDescending(x => x.Area).First();
                        panelListCopy.Remove(maxPanel);
                        #region Test Reversed OrderByDescending
                        //List<Panel> panelListCopyReversed = new List<Panel>(panelListCopy);
                        //panelListCopyReversed.Reverse();
                        //Panel maxPanel = panelListCopyReversed.OrderByDescending(x => x.Area).First();
                        #endregion

                        // place largest panel at the bottom of the bundle
                        bundle.Levels[numberOfLevels - 1].Add(maxPanel);
                        maxPanel.Column = 1;
                        maxPanel.Depth = 1;

                        // update bounds for bottom most level
                        bundleWidthBounds = BundleTools.CreateWidthBounds(bundle.Levels[numberOfLevels - 1]);
                        bundleLengthBounds = BundleTools.CreateLengthBounds(bundle.Levels[numberOfLevels - 1]);

                        // bundle starting at level above bottom level and in reverse order of the panel list
                        for (int i = numberOfLevels - 2; i >= 0; i--)
                        {
                            Level level = bundle.Levels[i];
                            colIndex = 1;
                            int depthIndex = 1;
                            for (int j = panelListCopy.Count - 1; j >= 0; j--)
                            {
                                if (panelListCopy.Count != 0)
                                {
                                    Panel panel = panelListCopy[j];

                                    if (level.Panels.Count < Settings.MaxPanelsPerLevel)
                                    {
                                        if ((panel.Width.AsDouble + level.Width) < bundleWidthBounds[1]  && panel.Height.AsDouble < bundleLengthBounds[1])
                                        {
                                            if (level.Add(panel))
                                            {
                                                panelListCopy.RemoveAt(j);
                                                panel.Column = colIndex;
                                                colIndex++;
                                                panel.Depth = depthIndex;
                                            }
                                            else
                                                throw new Exception("Cannot add panel to level");
                                        }
                                        #region Test Bound Preference
                                        // Prefer to place panel that is within both bounds, but if not possible place panel less than upperbounds
                                        //if (((panel.Width.AsDouble + level.Width) < bundleWidthBounds[1] && (panel.Width.AsDouble + level.Width) > bundleWidthBounds[0]) && (panel.Height.AsDouble < bundleLengthBounds[1] && panel.Height.AsDouble > bundleLengthBounds[0]))
                                        //{
                                        //    level.Add(panel);
                                        //    panelListCopy.RemoveAt(j);
                                        //}
                                        //if (level.Panels.Count == 0)
                                        //{
                                        //    for (int k = panelListCopy.Count - 1; k >= 0; k--)
                                        //    {
                                        //        if (panelListCopy.Count != 0)
                                        //        {
                                        //            panel = panelListCopy[k];
                                        //            if ((panel.Width.AsDouble + level.Width) < bundleWidthBounds[1] && panel.Height.AsDouble < bundleLengthBounds[1])
                                        //            {
                                        //                level.Add(panel);
                                        //                panelListCopy.RemoveAt(k);
                                        //                j--;
                                        //            }
                                        //        }
                                        //    }

                                        //}
                                        #endregion
                                    }
                                }
                            }
                            // We already have a bottom panel

                            // Keep track of level length and width before the additions
                            double originalLength = level.Length;
                            double originalWidth = level.Width;

                            // This will be used to add to the originalLength to get the overall level length
                            double currentLength = 0;
                            double currentWidth = 0;

                            // Find how much length area is remaining [level.length - bundleLengthBound[1]]
                            double lengthRemaining = bundleLengthBounds[1] - level.Length;

                            colIndex = 1;
                            depthIndex++;

                            for (int j = panelListCopy.Count - 1; j >= 0; j--)
                            {
                                Panel panel = panelListCopy[j];
                                if (panel.Height.AsDouble < lengthRemaining && (panel.Width.AsDouble + currentWidth) < bundleWidthBounds[1])
                                {
                                    // We can place the panel long ways with level
                                    if (level.Add(panel))
                                    {
                                        panelListCopy.RemoveAt(j);
                                        panel.Column = colIndex;
                                        colIndex++;
                                        panel.Depth = depthIndex;
                                    }
                                    else
                                        throw new Exception("Cannot add panel to level");

                                    // keep track of largest panel height and width
                                    if (panel.Height.AsDouble > currentLength)
                                        currentLength = panel.Height.AsDouble;

                                    currentWidth += panel.Width.AsDouble;
                                }

                                level.Length = originalLength + currentLength;
                                level.Width = originalWidth;
                            }

                            // update bounds
                            //bundleWidthBounds = BundleTools.CreateWidthBounds(level);
                            //bundleLengthBounds = BundleTools.CreateLengthBounds(level);
                        }
                    }
                }

                #endregion

                #region Fill In Bundle Information

                // Fill in bundle information
                double height = 0, weight = 0;
                foreach (Level levelInBundle in bundle.Levels)
                {
                    height += levelInBundle.Height;
                    weight += levelInBundle.Weight;
                }

                // Add sticker
                height += 3;

                bundle.Height = height;
                bundle.Weight = weight;
                bundle.Width = bundle.Levels.Max(x => x.Width);
                bundle.Length = bundle.Levels.Max(x => x.Length);
                bundle.NumberOfLevels = bundle.Levels.Count;

                Project.Bundles.Add(bundle);
                Project.CurrentBundleNumber++;

                #endregion
            }
        }
    }
}
