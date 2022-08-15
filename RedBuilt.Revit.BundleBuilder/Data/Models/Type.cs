using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.Data.Models
{
    public class Type
    {
        public string Name { get; }
        public string Symbol { get; }
        public static string TypeFamily { get; }
        public static Plate Plate { get; }

        public Type(string panelName)
        {
            Symbol = panelName[0].ToString();
            
            switch (Symbol)
            {
                case "E":
                    Name = "Exterior";
                    break;
                case "I":
                    Name = "Interior";
                    break;
                case "S":
                    Name = "Steel";
                    break;
                case "P":
                    Name = "Parapet";
                    break;
                case "F":
                    Name = "Furring";
                    break;
                default:
                    Name = "miscellaneous";
                    break;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
