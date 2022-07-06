using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Panel = RedBuilt.Revit.BundleBuilder.Data.Models.Panel;
using RedBuilt.Revit.BundleBuilder.Application.Sort;
using RedBuilt.Revit.BundleBuilder.Data.Models;

namespace RedBuilt.Revit.BundleBuilder.Data.Services
{
    public class RevitImportService
    {
        /// <summary>
        /// Parameter model
        /// </summary>
        //protected class Parameter
        //{
        //    public string Name { get; set; }
        //    public bool IsShared { get; set; }
        //    public Guid Guid { get; set; }
        //    public StorageType StorageType { get; set; }
        //    public Parameter(string parameterName, Element element)
        //    {
        //        Name = parameterName;
        //        ParameterSet parameterSet = element.Parameters;
        //        foreach (Autodesk.Revit.DB.Parameter parameter in parameterSet)
        //        {
        //            if (parameter.Definition.Name.Equals(parameterName))
        //            {
        //                IsShared = parameter.IsShared;

        //                if (IsShared)
        //                    Guid = parameter.GUID;
        //                else
        //                    Guid = Guid.Empty;
                        
        //                StorageType = parameter.StorageType;
        //                break;
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// List of panel parameter names
        /// </summary>
        private static readonly Dictionary<string, string> ParameterNameAndGuid = new Dictionary<string, string>
        {
            { "Assembly Height", "be9990f3-9c7f-4732-a147-c6bf839f92bf"},
            { "Assembly Length", "f35bfde6-7ea9-4cf2-b07d-a06d9c98a572" },
            { "Assembly Depth", "381d4274-c8f0-4b79-ba9b-683f7aab0e21" },
            { "Framing Member Mass", "70be58b1-bbbc-4199-9d16-d0af4969f2af" }
        };

        /// <summary>
        /// List of parameter objects
        /// </summary>
        //private static List<Parameter> Parameters = new List<Parameter>();

        /// <summary>
        /// Filters revit document for wall panel elements and creates a list of panel objects
        /// </summary>
        /// <param name="doc">revit document</param>
        /// <returns>list of sorted panels</returns>
        public static IEnumerable<Panel> GetPanels(Document doc)
        {
            List<Panel> panels = new List<Panel>();

            // Filter elements by "Structural Framing Assembly" to a list
            ElementId elementId = new ElementId(BuiltInParameter.ELEM_FAMILY_PARAM);
            ParameterValueProvider pvp = new ParameterValueProvider(elementId);
            FilterStringRuleEvaluator fsrv = new FilterStringEquals();
            FilterRule fr = new FilterStringRule(pvp, fsrv, "Structural Framing Assembly", true);
            ElementParameterFilter paramFilter = new ElementParameterFilter(fr, false);

            List<Element> panelElements = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Assemblies)
                .WhereElementIsNotElementType().WherePasses(paramFilter).ToList();

            // Exceptions
            if (panelElements.Count == 0)
                throw new Exception(); // PanelsNotFoundException()

            // Create parameters
            //foreach (string parameterName in ParameterNames)
            //    Parameters.Add(new Parameter(parameterName, panelElements[0]));
            
            // Create new Panels using the filtered elements
            foreach (Element panelElement in panelElements)
            {
                // Exceptions
                if (panelElement == null)
                    throw new Exception(); // PanelNotFoundException(panelId)
                if (String.IsNullOrEmpty(panelElement.Name))
                    throw new Exception(); // PanelAttributeNotFoundException(panelId, name)

                // Create the panel
                Panel panel = new Panel(panelElement);

                // Populate each field (Width, Height, Weight, Plate, Type)
                foreach (KeyValuePair<string, string> parameterNameGuidPair in ParameterNameAndGuid)
                {
                    // Find the revit parameter
                    Autodesk.Revit.DB.Parameter p;
                    if (String.IsNullOrEmpty(parameterNameGuidPair.Value))
                        p = panel.Element.LookupParameter(parameterNameGuidPair.Key);
                    else
                        p = panel.Element.get_Parameter(new Guid(parameterNameGuidPair.Value));

                    // Exception
                    if (p == null)
                        throw new Exception(); // PanelAttributeNotFoundException(panelId, parameterNameGuidPair.Key)

                    switch (parameterNameGuidPair.Key)
                    {
                        case "Assembly Height":
                            panel.Height = new Height(Math.Round(p.AsDouble() * 12, 3));
                            break;
                        case "Assembly Length":
                            panel.Width = new Width(Math.Round(p.AsDouble() * 12, 3));
                            break;
                        case "Assembly Depth":
                            panel.Plate = new Plate(Math.Round(p.AsDouble() * 12, 3));
                            break;
                        case "Framing Member Mass":
                            panel.Weight = Math.Round(p.AsDouble());
                            break;
                    }

                    panel.Type = new Models.Type(panel.Name);
                }

                // Switch width and height if the panel is sideways
                if (panel.Width.AsDouble > panel.Height.AsDouble)
                {
                    double temp = panel.Width.AsDouble;
                    panel.Width = new Width(panel.Height.AsDouble);
                    panel.Height = new Height(temp);
                }

                panels.Add(panel);
            }

            // Sort Panels by name
            panels = PanelNameSort.Sort(panels);

            return panels;
        }
    }
}
