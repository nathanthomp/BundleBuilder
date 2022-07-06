using RedBuilt.Revit.BundleBuilder.Application.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.Data.Models
{
    public class Plate
    {
        public string Description { get;}
        public double Width { get; }
        public string WidthString { get; }

        public Plate(double plateWidth)
        {
            Width = plateWidth;
            WidthString = DimensionTool.AsString(Width);
        }
    }
}
