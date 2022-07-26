using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Panel = RedBuilt.Revit.BundleBuilder.Data.Models.Panel;
using RedBuilt.Revit.BundleBuilder.Application.Sort;
using RedBuilt.Revit.BundleBuilder.Data.Models;
using System.Windows;

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
        /// List of panel parameter names and their guid in revit
        /// </summary>
        private static readonly Dictionary<string, string> ParameterNameAndGuid = new Dictionary<string, string>
        {
            { "Assembly Height", "be9990f3-9c7f-4732-a147-c6bf839f92bf"},
            { "Assembly Length", "f35bfde6-7ea9-4cf2-b07d-a06d9c98a572" },
            { "Assembly Depth", "381d4274-c8f0-4b79-ba9b-683f7aab0e21" },
            { "Framing Member Mass", "70be58b1-bbbc-4199-9d16-d0af4969f2af" },
            { "Assembly Area", "99a0d818-1d3b-4954-a777-e87ab3f3d5c8" }
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
        public static List<Panel> GetPanels(Document doc)
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

            // Filter elements by "Wall" to a list
            List<Element> wallElements = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Walls)
                .OfClass(typeof(Wall)).WhereElementIsNotElementType().ToList();

            // Exceptions
            if (panelElements.Count == 0)
                throw new Exception("No \"Structural Framing Assembly\" elements found.");
            if (wallElements.Count == 0)
                throw new Exception("No \"Wall\" elments found.");

            // Create parameters
            //foreach (string parameterName in ParameterNames)
            //    Parameters.Add(new Parameter(parameterName, panelElements[0]));
            
            // Create new Panels using the filtered elements
            foreach (Element panelElement in panelElements)
            {
                // Exceptions
                if (panelElement == null)
                    throw new Exception("Panel not found");
                if (String.IsNullOrEmpty(panelElement.Name))
                    throw new Exception("Panel name not found on element: " + panelElement.Id);


                // Find matching wall and panel elements
                Element wallElement = wallElements.Where(x => x.Name == panelElement.Name).First();

                // Exceptions
                if (wallElement == null)
                    throw new Exception("Panel not found");
                if (String.IsNullOrEmpty(wallElement.Name))
                    throw new Exception("Panel name not found on element: " + wallElement.Id);

                // Create the panel
                Panel panel = new Panel(panelElement, wallElement);









                // Populate each field (Width, Height, Weight, Plate, Type)
                foreach (KeyValuePair<string, string> parameterNameGuidPair in ParameterNameAndGuid)
                {
                    // Find the revit parameter
                    Parameter p;
                    if (String.IsNullOrEmpty(parameterNameGuidPair.Value))
                        p = panel.PanelElement.LookupParameter(parameterNameGuidPair.Key);
                    else
                        p = panel.PanelElement.get_Parameter(new Guid(parameterNameGuidPair.Value));

                    // Exception
                    if (p == null)
                        throw new Exception("Panel attribute " + parameterNameGuidPair.Key + " not found on element: " + panelElement.Id);
                    if (p.AsDouble() <= 0)
                        throw new Exception("Panel element: " + panelElement.Id + " with attribute " + p.Definition + " is not defined.");
                    if (HasDuplicatePanel(panel, panels))
                        throw new Exception("Panel element: " + panelElement.Id + " has duplicate names");

                    panel.Type = new Models.Type(panel.Name.FullName);

                    switch (parameterNameGuidPair.Key)
                    {
                        case "Assembly Height":
                            panel.Height = new Height(Math.Round(p.AsDouble() * 12, 3));
                            break;
                        case "Assembly Length":
                            panel.Width = new Width(Math.Round(p.AsDouble() * 12, 3));
                            break;
                        case "Assembly Depth":
                            panel.Plate = new Plate(Math.Round(p.AsDouble() * 12, 3), panel.Type.Name);
                            break;
                        case "Framing Member Mass":
                            panel.Weight = Math.Round(p.AsDouble());
                            break;
                        case "Assembly Area":
                            panel.Area = Math.Round(p.AsDouble());
                            break;
                    }

                }

                // Exceptions
                if (panel.Plate.Description == null)
                    throw new Exception("Panel element: " + panelElement.Id + " with attribute Assembly Depth is not accurate.");

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="panels"></param>
        /// <returns></returns>
        public static bool HasDuplicatePanel(Panel panel, List<Panel> panels)
        {
            bool result = false;
            foreach (Panel p in panels)
                if (panel.Name.FullName.Equals(p.Name.FullName))
                    result = true;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void GetProject(Document doc)
        {
            List<Element> viewSheetSet = new FilteredElementCollector(doc).OfClass(typeof(ViewSheet)).ToList();
            ViewSheet projectCoverPage = (ViewSheet)viewSheetSet.First(x => x.Name == "Project Cover Page");

            // Project Number
            if (doc.ProjectInformation.Number == null || doc.ProjectInformation.Number.Trim(' ').Length == 0)
                throw new Exception("Project number is missing");
            else
                Project.Number = doc.ProjectInformation.Number;

            // Project Name
            ParameterSet parameterSet = doc.ProjectInformation.Parameters;
            foreach (Parameter parameter in parameterSet)
            {
                if (parameter.Definition.Name.Equals("Project Name"))
                {
                    Project.Name = parameter.AsString();
                    break;
                }
                   
            }
            if (String.IsNullOrEmpty(Project.Name))
                throw new Exception("Cannot find project name");

            // Project Location
            if (doc.ProjectInformation.Address == null)
                throw new Exception("No location data availible.");
            else
            {
                string address = doc.ProjectInformation.Address;

                if (address.Contains(Environment.NewLine))
                {
                    int indexOfNewLine = address.IndexOf(Environment.NewLine) + 2;
                    Project.Location = address.Substring(indexOfNewLine, (address.Length - 6) - indexOfNewLine);
                }
                else
                    Project.Location = address;

            }

            // Correct project information
            if (Project.Name.Contains(Project.Number))
            {
                // Take project number out of project name
                int indexOfNumber = Project.Name.IndexOf(Project.Number);
                Project.Name = Project.Name.Remove(indexOfNumber, 6);
            }

            if (Project.Name.Length > 30)
                // trim down the string
                Project.Name = Project.Name.Substring(0, 30);

            if (Project.Location.Length > 30)
                // trim down the string
                Project.Location = Project.Location.Substring(0, 30);
            
        }

    }
}
