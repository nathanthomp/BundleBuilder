using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.Data.Models
{
    public class Bundle
    {
        public int Number { get; set; }
        public int NumberOfLevels { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }
        public double Weight { get; set; }

        // ???
        public string Type { get; set; }
        
        // ???
        public string Plate { get; set; }
        public List<Level> Levels { get; set; } = new List<Level>();

        public Bundle(int bundleNumber, int bundleNumberOfLevels)
        {
            Number = bundleNumber;
            NumberOfLevels = bundleNumberOfLevels;
        }

        public void Add(Level level)
        {
            if (!Levels.Contains(level))
            {
                Levels.Add(level);
                Weight += level.Weight;
                level.Bundle = this;
                if (level.Length > Length)
                    Length = level.Length;
                Height += level.Height;
                if (level.Width > Width)
                    Width = level.Width;
            }
        }

        public void Remove(Level level)
        {
            if (Levels.Contains(level))
            {
                Levels.Remove(level);
                Weight -= level.Weight;
                NumberOfLevels--;
                if (Levels.Count > 0)
                    Length = Levels.Max(x => x.Length);
                else
                    Length = 0;
                Height -= level.Height;
                if (Levels.Count > 0)
                    Width = Levels.Max(x => x.Width);
                else
                    Width = 0;
            }
        }

        public override string ToString()
        {
            return Number.ToString();
        }

    }
}
