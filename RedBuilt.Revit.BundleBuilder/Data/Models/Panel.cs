﻿using Autodesk.Revit.DB;
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
        public Element PanelElement { get; set; }
        public Element WallElement { get; set; }
        public int Column { get; set; }
        public int Depth { get; set; }

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

        public Panel(Element panelElement, Element wallElement)
        {
            PanelElement = panelElement;
            WallElement = wallElement;
            Name = new Name(panelElement.Name);
            _toBundle = true;
        }

        public Panel(Element panelElement)
        {
            PanelElement = panelElement;
            Name = new Name(panelElement.Name);
            _toBundle = true;
        }

        public bool IsWithinBounds(double[] widthBounds, double[] lengthBounds)
        {
            return (Height.AsDouble > lengthBounds[0] && Height.AsDouble < lengthBounds[1])
                && (Width.AsDouble > widthBounds[0] && Width.AsDouble < widthBounds[1]);
        }

        public override string ToString()
        {
            return Name.FullName;
        }
    }
}
