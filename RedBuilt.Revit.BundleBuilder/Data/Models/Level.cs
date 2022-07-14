using RedBuilt.Revit.BundleBuilder.Application.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.Data.Models
{
    public class Level
    {
        public int Number { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public Bundle Bundle { get; set; }
        public List<Panel> Panels { get; set; }

        public Level(int levelNumber)
        {
            Number = levelNumber;
            Panels = new List<Panel>();
        }

        // return boolean to ensure that add was successful
        public void Add(Panel panel)
        {
            if (!Panels.Contains(panel))
            {
                Panels.Add(panel);
                Width += panel.Width.AsDouble;
                Length = Panels.Max(x => x.Height.AsDouble);
                Weight += panel.Weight;
                panel.Level = this;
                panel.Bundle = Bundle;
                if (Height == 0)
                    Height = panel.Plate.Width;
            }
        }

        // return boolean to ensure that add was successful
        public void Remove(Panel panel)
        {
            if (Panels.Contains(panel))
            {
                Panels.Remove(panel);
                Width -= panel.Width.AsDouble;
                Weight -= panel.Weight;


                if (Panels.Count > 0)
                    Length = Panels.Max(x => x.Height.AsDouble);
                else
                    Length = 0;
            }
        }

        public override string ToString()
        {
            return Number.ToString();
        }
    }
}
