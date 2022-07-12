using RedBuilt.Revit.BundleBuilder.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.Application.Sort
{
    public class PanelTypeAndPlateSort
    {
        public static Dictionary<string, Dictionary<string, List<Panel>>> Sort()
        {
            Dictionary<string, Dictionary<string, List<Panel>>> result = new Dictionary<string, Dictionary<string, List<Panel>>>();

            List<string> types = new List<string>();

            // Create a list of types that are in the project
            foreach (Panel panel in Project.Panels)
            {
                if (!types.Contains(panel.Type.Name))
                {
                    types.Add(panel.Type.Name);
                }
            }

            // Create a dictionary element per type
            foreach (string type in types)
            {
                List<string> platesInType = new List<string>();

                Dictionary<string, List<Panel>> plateAndPanels = new Dictionary<string, List<Panel>>();

                // Create a list of plates that are in the type
                foreach (Panel panel in Project.Panels)
                {
                    if (panel.Type.Name == type)
                    {
                        if (!platesInType.Contains(panel.Plate.Description))
                        {
                            platesInType.Add(panel.Plate.Description);
                        }
                    }
                }

                // Fill in plateAndPanels dict
                foreach (string plate in platesInType)
                {
                    List<Panel> panels = new List<Panel>();
                    foreach (Panel panel in Project.Panels)
                    {
                        if (panel.Plate.Description.Equals(plate) && panel.Type.Name.Equals(type))
                        {
                            panels.Add(panel);
                        }
                    }
                    if (!plateAndPanels.ContainsKey(plate))
                    {
                        plateAndPanels.Add(plate, panels);
                    }
                    
                }

                result.Add(type, plateAndPanels);
            }

            return result;
        }
    }
}
