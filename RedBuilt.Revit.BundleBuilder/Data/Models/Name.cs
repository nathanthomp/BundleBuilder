using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.Data.Models
{
    public class Name
    {
        public string FullName { get; }
        public string Symbol { get; set; }
        public int Instance { get; set; }

        // might not need this
        public string Version { get; set; }

        public string InstanceAndVersion { get; set; }

        public Name(string fullName)
        {
            FullName = fullName;
            Symbol = fullName[0].ToString();
            InstanceAndVersion = fullName.Substring(1, fullName.Length - 1);

            int _instance = 0;

            char[] characters = InstanceAndVersion.ToCharArray();

            for (int i = 0; i < characters.Length; i++)
            {
                if (Char.IsDigit(characters[i]))
                {
                    _instance = _instance * 10 + (int)Char.GetNumericValue(characters[i]);
                }
                else if (Char.IsLetter(characters[i]))
                {
                    Version += characters[i].ToString();
                }
            }

            Instance = _instance;

            if (Version == null)
            {
                Version = "";
            }
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}
