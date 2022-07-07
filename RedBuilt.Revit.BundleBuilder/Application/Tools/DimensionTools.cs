using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.Application.Tools
{
    public class DimensionTools
    {
        public static string AsString(double dimension)
        {
            double feet = Math.Floor(dimension / 12);
            double inches = Math.Round(dimension % 12, 3);

            if (feet != 0)
                return feet.ToString() + "' - " + inches.ToString() + "\"";
            else
                return inches.ToString() + "\"";
        }

        public static int AsInt(double dimension)
        {
            return (int)Math.Round(dimension);
        }
    }
}
