using RedBuilt.Revit.BundleBuilder.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RedBuilt.Revit.BundleBuilder.Application.Tools
{
    public class LevelTools
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="levelName"></param>
        /// <returns></returns>
        public static Level GetLevelFromName(string levelName)
        {
            // example: Bundle 1 - Level 1
            int dashIndex = levelName.IndexOf('-');
            
            // Get string of the bundle and level numbers
            string levelNumberStr = levelName[dashIndex + 8].ToString();
            string bundleNumberStr = levelName.Substring(7 , (dashIndex - 7));

            // Get int of the bundle and level numbers
            if (Int32.TryParse(bundleNumberStr, out int bundleNumber)) { }
            if (Int32.TryParse(levelNumberStr, out int levelNumber)) { }

            // Get Bundle and Level instances
            Bundle bundle = Project.Bundles.Where(x => x.Number == bundleNumber).First();
            return bundle.Levels.Where(x => x.Number == levelNumber).First();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bundle"></param>
        /// <param name="levelNumber"></param>
        /// <returns></returns>
        public static Level GetLevelFromNumberAndBundle(Bundle bundle, int levelNumber)
        {
            return bundle.Levels.Where(x => x.Number == levelNumber).First();
        }

 
    }
}
