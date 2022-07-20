using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using RedBuilt.Revit.BundleBuilder.Application.Reports;
using RedBuilt.Revit.BundleBuilder.Application.Tools;
using RedBuilt.Revit.BundleBuilder.Data.Models;
using RedBuilt.Revit.BundleBuilder.Data.States;
using Panel = RedBuilt.Revit.BundleBuilder.Data.Models.Panel;

namespace RedBuilt.Revit.BundleBuilder.Data.Services
{
    public class DataService
    {

        /// <summary>
        /// Pickup a level and place a level elsewhere.
        /// </summary>
        /// <param name="level">the level to move</param>
        /// <param name="destBundle">the number representing the destination bundle</param>
        /// <param name="destLevel">the number representing the destination level</param>
        public static void ProcessModification(Level level, int destBundleNumber, int destLevelNumber)
        {
            // Remove the level from its bundle
            RemoveFromBundle(level);

            // Add the level to bundle
            AddLevelToBundle(level, destBundleNumber, destLevelNumber);

            // Update the view
            Update();
        }

        /// <summary>
        /// Removes level from bundle, removes bundle from project if there are no more levels after deletion
        /// </summary>
        /// <param name="level">level to delete</param>
        private static void RemoveFromBundle(Level level)
        {
            // Get the current bundle that the level is in
            Bundle currentBundle = level.Bundle;

            // Remove level from the current bundle
            currentBundle.Remove(level);

            // If there are no more levels left in the bundle, remove the bundle from 
            // the project and corrent the bundle numbers
            if (currentBundle.Levels.Count == 0)
            {
                Project.Remove(currentBundle);
                BundleTools.CorrectBundleNumbers();
            }

            // Correct the level's number in the bundle
            BundleTools.CorrectLevelNumbers(currentBundle);  
        }

        /// <summary>
        /// Adds level to bundle, if level number already exists, adds beneath that level
        /// </summary>
        /// <param name="level">level to add</param>
        /// <param name="destBundleNumber">bundle to add level to</param>
        /// <param name="destLevelNumber">level location to add level to</param>
        private static void AddLevelToBundle(Level level , int destBundleNumber, int destLevelNumber)
        {
            // Add the data to the level
            Bundle destBundle = Project.Bundles.Where(x => x.Number == destBundleNumber).First();
            level.Bundle = destBundle;
            level.Number = destLevelNumber;

            // Determine if there is a level in the destination level that has the same number
            bool existingLevelHasSameNumber = false;
            foreach (Level exsistingLevel in destBundle.Levels)
                if (exsistingLevel.Number == destLevelNumber)
                    existingLevelHasSameNumber = true;

            // if bundle has a level with the same number increment those levels whose
            // number is greater than or equal to the destination level number
            if (existingLevelHasSameNumber)
            {
                foreach (Level exsistingLevel in destBundle.Levels.Where(x => x.Number >= destLevelNumber))
                    exsistingLevel.Number++;
                destBundle.Add(level);

            }
            else
                destBundle.Add(level);

            BundleTools.CorrectLevelNumbers(destBundle);
        }

        /// <summary>
        /// Creates updated bundle report and refreshes the export view
        /// </summary>
        private static void Update()
        {
            BundleReport.Export();
            ProjectState.ExportView.createdBundles.Refresh();
        }
    }
}
