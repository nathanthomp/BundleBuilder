using RedBuilt.Revit.BundleBuilder.Application.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.Data.Models
{
    public class Height
    {
        public string AsString { get; }
        public double AsDouble { get; set; }
        public int AsInt { get; }

        public Height(double height)
        {
            AsDouble = height;
            AsString = DimensionTools.AsString(height);
            AsInt = DimensionTools.AsInt(height);
        }
    }
}
