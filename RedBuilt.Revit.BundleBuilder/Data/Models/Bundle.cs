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

        /// <summary>
        /// Bundle constructor
        /// </summary>
        /// <param name="bundleNumber">initial number</param>
        /// <param name="bundleNumberOfLevels">initial number of levels</param>
        public Bundle(int bundleNumber, int bundleNumberOfLevels)
        {
            Number = bundleNumber;
            NumberOfLevels = bundleNumberOfLevels;
        }

        /// <summary>
        /// Adds a level to this bundle
        /// </summary>
        /// <param name="level">the level to add</param>
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

        /// <summary>
        /// Removes a level from this bundle
        /// </summary>
        /// <param name="level">the level to remove</param>
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

        /// <summary>
        /// Bundle object ToString() method
        /// </summary>
        /// <returns>proper bundle number, bundle type and bundle plate</returns>
        public override string ToString()
        {
            string properNumber = string.Empty;
            if (Number < 10)
                properNumber = String.Format("0{0}", Number.ToString());
            else
                properNumber = Number.ToString();

            return String.Format("B{0} {1} - {2}", properNumber, Type, Plate);
        }

    }
}
