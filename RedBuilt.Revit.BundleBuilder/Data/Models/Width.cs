using RedBuilt.Revit.BundleBuilder.Application.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.Data.Models
{
    public class Width
    {
        public string AsString { get; }
        public double AsDouble { get; set; }
        public int AsInt { get; }

        public Width(double width)
        {
            AsDouble = width;
            AsString = DimensionTools.AsString(width);
            AsInt = DimensionTools.AsInt(width);
        }
    }
}
