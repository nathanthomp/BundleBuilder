using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.Data.Models
{
    public class Panel : ObservableObject
    {
        public Name Name { get; set; }
        public Width Width { get; set; }
        public Height Height { get; set; }
        public double Weight { get; set; }
        public double Area { get; set; }
        public Plate Plate { get; set; }
        public Type Type { get; set; }
        public Truck Truck { get; set; }
        public Bundle Bundle { get; set; }
        public Level Level { get; set; }
        public Element StructWall { get; set; }
        public Element BasicWall { get; set; }
        public int Column { get; set; }
        public int Depth { get; set; }

        /// <summary>
        /// Property to determine if this panel will be included in the bundles
        /// </summary>
        private bool _toBundle;
        public bool ToBundle
        {
            get { return _toBundle; }
            set
            {
                _toBundle = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Panel constructor
        /// </summary>
        /// <param name="basicWall">revit wall assembly element</param>
        public Panel(Element basicWall)
        {
            BasicWall = basicWall;
            Name = new Name(basicWall.LookupParameter("Mark").AsString());
            _toBundle = true;
        }

        /// <summary>
        /// Panel constructor
        /// </summary>
        /// <param name="panelElement">revit wall assembly element</param>
        /// <param name="wallElement">revit generic wall element</param>
        public Panel(Element basicWall, Element structWall)
        {
            BasicWall = basicWall;
            StructWall = structWall;
            Name = new Name(structWall.Name);
            _toBundle = true;
        }

        /// <summary>
        /// Determines if the panel is within the criteria for width and length
        /// </summary>
        /// <param name="widthBounds">width upper and lower bound</param>
        /// <param name="lengthBounds">length upper and lower bound</param>
        /// <returns>true if within bounds, otherwisw false</returns>
        public bool IsWithinBounds(double[] widthBounds, double[] lengthBounds)
        {
            return (Height.AsDouble > lengthBounds[0] && Height.AsDouble < lengthBounds[1])
                && (Width.AsDouble > widthBounds[0] && Width.AsDouble < widthBounds[1]);
        }

        /// <summary>
        /// Panel object ToString() method
        /// </summary>
        /// <returns>panel full name</returns>
        public override string ToString()
        {
            return Name.FullName;
        }
    }
}
