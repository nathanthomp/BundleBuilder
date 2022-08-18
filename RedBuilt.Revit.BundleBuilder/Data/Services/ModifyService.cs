using RedBuilt.Revit.BundleBuilder.Application.Reports;
using RedBuilt.Revit.BundleBuilder.Application.Tools;
using RedBuilt.Revit.BundleBuilder.Data.Models;
using RedBuilt.Revit.BundleBuilder.Data.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.Data.Services
{
    public static class ModifyService
    {
        public static string ErrorMessage = "";

        public static bool ProcessModification(string moveOption, string moveObject, int bundleDest, int levelDest, bool newLevel)
        {
            if (DataIsValid(moveOption, moveObject, bundleDest, levelDest))
            {
                // Find current data and remove from current location
                if (moveOption.Equals("level"))
                {
                    Level level = LevelTools.GetLevelFromName(moveObject);
                    Bundle currentBundle = level.Bundle;

                    
                    Bundle destinationBundle = Project.Bundles.Where(x => x.Number == bundleDest)?.First();
                    if (destinationBundle == null)
                    {
                        destinationBundle = new Bundle(bundleDest);
                        Project.Bundles.Add(destinationBundle);
                    }
                        

                    currentBundle.Remove(level);
                    LevelTools.CorrectLevelNumbers(currentBundle);
                    if (currentBundle.Levels.Count == 0)
                    {
                        Project.Remove(currentBundle);
                        BundleTools.CorrectBundleNumbers();
                    }

                    level.Number = levelDest;

                    

                    // Find the levels creater than levelDest
                    // Increment them by 1
                    List<Level> levelsToIncrease = new List<Level>();
                    levelsToIncrease = destinationBundle.Levels.Where(x => x.Number > levelDest || x.Number == levelDest).ToList();
                    foreach (Level levelToIncrease in levelsToIncrease)
                        levelToIncrease.Number += 1;

                    // Add level to new destination
                    destinationBundle.Add(level);
                    destinationBundle.NumberOfLevels++;

                    LevelTools.CorrectLevelNumbers(destinationBundle);

                }
                if (moveOption.Equals("panel"))
                {
                    Panel panel = PanelTools.GetPanelFromName(moveObject);
                    Level currentLevel = panel.Level;
                    Bundle currentBundle = panel.Bundle;

                    
                    

                    Bundle destinationBundle = Project.Bundles.Where(x => x.Number == bundleDest)?.First();
                    if (destinationBundle == null)
                    {
                        destinationBundle = new Bundle(bundleDest);
                    }

                    currentLevel.Remove(panel);
                    if (currentLevel.Panels.Count == 0)
                    {
                        currentBundle.Remove(currentLevel);
                        LevelTools.CorrectLevelNumbers(currentBundle);
                        if (currentBundle.Levels.Count == 0)
                        {
                            Project.Remove(currentBundle);
                            BundleTools.CorrectBundleNumbers();
                        }
                    }

                    if (newLevel)
                    {
                        Level level = new Level(levelDest);
                        level.Add(panel);
                        panel.Depth = 1;
                        panel.Column = 1;

                        panel.Bundle = destinationBundle;

                        List<Level> levelsToIncrease = new List<Level>();
                        levelsToIncrease = destinationBundle.Levels.Where(x => x.Number > levelDest || x.Number == levelDest).ToList();
                        foreach (Level levelToIncrease in levelsToIncrease)
                            levelToIncrease.Number += 1;

                        destinationBundle.Add(level);

                        LevelTools.CorrectLevelNumbers(destinationBundle);
                    }
                    else
                    {
                        if (levelDest > destinationBundle.Levels.Count)
                        {
                            ErrorMessage = "Cannot find a location to place " + moveObject;
                            return false;
                        }

                        Level destinationLevel = destinationBundle.Levels.Where(x => x.Number == levelDest).First();

                        if (destinationLevel.Panels.Max(x => x.Depth) > 1)
                        {
                            ErrorMessage = "Cannot add to level with a depth of 2.";
                            return false;
                        }

                        panel.Depth = 1;
                        panel.Column = destinationLevel.Panels.Max(x => x.Column) + 1;
                        destinationLevel.Add(panel);
                    }


                    // take the remaining panels in the level
                    // for (number of panels left)
                    //  take smallest and make it the first column

                    List<Panel> currentLevelPanelsCopy = new List<Panel>(currentLevel.Panels);
                    for (int i = 1; i <= currentLevelPanelsCopy.Count; i++)
                    {
                        Panel remainingPanel = currentLevelPanelsCopy.OrderBy(x => x.Width).Min();
                        remainingPanel.Column = i;
                        currentLevelPanelsCopy.Remove(remainingPanel);
                    }


                }

                BundleReport.CreateHtml();
                ProjectState.ExportView.RefreshView();

                return true;
            }
            else
            {
                ErrorMessage = "Cannot find a location to place this " + moveOption;
                return false;
            }
        }

        private static bool DataIsValid(string moveOption, string moveObject, int bundleDest, int levelDest)
        {
            bool result = false;
            Bundle bundle = Project.Bundles.Where(x => x.Number == bundleDest).First();

            if (bundle != null)
            {
                if (bundleDest > 0 && bundleDest <= Project.Bundles.Count + 1)
                {
                    if (levelDest > 0 && levelDest <= bundle.Levels.Count + 1)
                    {
                        if (!String.IsNullOrEmpty(moveOption) && !String.IsNullOrEmpty(moveObject))
                        {
                            result = true;
                        }
                    }
                }
            }
            return result;
        }

    }
}
