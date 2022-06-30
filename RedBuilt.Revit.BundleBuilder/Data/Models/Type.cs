using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.Data.Models
{
    public class Type
    {
        public string Name { get; set; }
        public string Symbol { get; set; }

        public Type(string panelName)
        {
            Symbol = panelName[0].ToString();
        }
    }
}
