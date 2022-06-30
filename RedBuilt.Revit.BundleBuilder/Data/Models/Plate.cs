using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.Data.Models
{
    public class Plate
    {
        public string Description { get; set; }
        public double Width { get; set; }

        public Plate(double plateWidth)
        {
            Width = plateWidth;
        }
    }
}
