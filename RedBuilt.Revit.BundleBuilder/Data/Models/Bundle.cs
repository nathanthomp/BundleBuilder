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
        public double Weight { get; set; }
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
            }
            level.Bundle = this;
        }
        public void Remove(Level level)
        {
            if (Levels.Contains(level))
            {
                Levels.Remove(level);
                Weight -= level.Weight;
                NumberOfLevels--;
            }
        }

    }
}
