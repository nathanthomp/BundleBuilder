using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.Data.Models
{
    public class Truck
    {
        public int Number { get; set; }
        public List<Bundle> Bundles { get; set; } = new List<Bundle>();

        public Truck(int truckNumber)
        {
            Number = truckNumber;
        }
    }
}
