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
        public string PlateMaterial { get; }

        public Plate(double plateWidth, string plateMaterial)
        {
            Width = plateWidth;
            WidthString = DimensionTools.AsString(Width);
            PlateMaterial = plateMaterial;

            if (PlateMaterial.Equals("Steel"))
            {
                if (Width < 5)
                {
                    Description = "400";
                }
                else if (Width < 6)
                {
                    Description = "550";
                }
                else if (Width < 8)
                {
                    Description = "600";
                }
                else if (Width < 10)
                {
                    Description = "800";
                }
                else if (Width < 12)
                {
                    Description = "1000";
                }
                else if (Width < 14)
                {
                    Description = "1200";
                }
            }
            else
            {
                if (Width < 5.5)
                {
                    Description = "2x4";
                }
                else if (Width < 7.25)
                {
                    Description = "2x6";
                }
                else if (Width < 9.15)
                {
                    Description = "2x8";
                }
                else if (Width < 11.25)
                {
                    Description = "2x10";
                }
                else if (Width < 13.25)
                {
                    Description = "2x12";
                }
            }

        }
    }
}
