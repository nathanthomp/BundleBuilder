using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.Data.Models
{
    public class Panel
    {
        public string Name { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public Plate Plate { get; set; }
        // heads up
        public Type Type { get; set; }
        public Truck Truck { get; set; }
        public Bundle Bundle { get; set; }
        public Level Level { get; set; }
        public Element Element { get; set; }

        public Panel(Element element)
        {
            Element = element;
            Name = element.Name;
        }

        public bool IsWithinBounds(double[] widthBounds, double[] lengthBounds)
        {
            return (this.Height > lengthBounds[0] && this.Height < lengthBounds[1])
                && (this.Width > widthBounds[0] && this.Width < widthBounds[1]);
        }
    }
}
