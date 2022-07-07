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

        public void Add(Panel panel)
        {
            if (!Panels.Contains(panel))
            {
                Panels.Add(panel);
                Width += panel.Width.AsDouble;
                if (panel.Height.AsDouble > Length)
                {
                    Length = panel.Height.AsDouble;
                }
                Weight += panel.Weight;
                panel.Level = this;
                panel.Bundle = Bundle;
            }
        }
        public void Remove(Panel panel)
        {
            if (Panels.Contains(panel))
            {
                Panels.Remove(panel);
                Width -= panel.Width.AsDouble;
                // Get Height of new longest panel in level
                Weight -= panel.Weight;
            }
        }
    }
}
