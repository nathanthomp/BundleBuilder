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
using Type = RedBuilt.Revit.BundleBuilder.Data.Models.Type;
using RedBuilt.Revit.BundleBuilder.Data.States;
using System.IO;

namespace RedBuilt.Revit.BundleBuilder.Data.Services
{
    public class RevitImportService
    {

        public static string ErrorMessage { get; private set; }

        public static bool Import(Document doc)
        {
            return (GetPanels(doc) && GetProject(doc));
        }

        #region Parameter class

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
        /// List of parameter objects
        /// </summary>
        //private static List<Parameter> Parameters = new List<Parameter>();

        #endregion

        /// <summary>
        /// List of panel parameter names in revit
        /// </summary>
        private static readonly List<string> ParamNames = new List<string>
        {
            "Assembly Height",
            "Assembly Length",
            "Assembly Depth",
            "Assembly Area",
            "Framing Member Mass"
        };

        /// <summary>
        /// Ensures that the configuration of panels is correct,
        /// then imports data for every panel
        /// </summary>
        /// <param name="doc">revit document</param>
        /// <returns>true if import success, false otherwise</returns>
        private static bool GetPanels(Document doc)
        {
            // Only Basic walls & RB Fields = GOOD
            // Only Basic walls & no RB Fields = BAD
            // Both walls = GOOD

            bool hasBasicWalls = false;
            bool hasStructWalls = false;
            bool hasRBFields = false;
            List<Panel> panels = new List<Panel>();

            // Get both panels
            ElementId elementId = new ElementId(BuiltInParameter.ELEM_FAMILY_PARAM);
            ParameterValueProvider pvp = new ParameterValueProvider(elementId);
            FilterStringRuleEvaluator fsrv = new FilterStringEquals();
            FilterRule fr = new FilterStringRule(pvp, fsrv, "Structural Framing Assembly", true);
            ElementParameterFilter paramFilter = new ElementParameterFilter(fr, false);
            List<Element> structWallElements = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Assemblies)
                .WhereElementIsNotElementType().WherePasses(paramFilter).ToList();

            List<Element> basicWallElements = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Walls)
                .OfClass(typeof(Wall)).WhereElementIsNotElementType().ToList();

            // Determine if viable fields have been found
            if (basicWallElements.Count > 0)
                hasBasicWalls = true;
            if (structWallElements.Count > 0)
                hasStructWalls = true;

            if (!hasBasicWalls)
            {
                // No walls found
                ErrorMessage = "Unviable Setup: Cannot find wall elements.";
                return false;
            }

            Parameter a = basicWallElements.First().LookupParameter("RB Bundle");
            Parameter b = basicWallElements.First().LookupParameter("RB Bundle Level");
            Parameter c = basicWallElements.First().LookupParameter("RB Bundle Column");
            Parameter d = basicWallElements.First().LookupParameter("RB Bundle Depth");

            //if (a != null && b != null && c != null && d != null)
            //    hasRBFields = true;

            // Make list of parameters to create
            List<string> parametersToCreate = new List<string>();
            if (a == null)
                parametersToCreate.Add("RB Bundle");
            if (b == null)
                parametersToCreate.Add("RB Bundle Level");
            if (c == null)
                parametersToCreate.Add("RB Bundle Column");
            if (d == null)
                parametersToCreate.Add("RB Bundle Depth");


            // Gate - assuming panel data is correct,
            // ensure that there is a viable option to export to revit
            if (!hasRBFields)
            {
                // Create RB Fields
                CreateSharedParameters(basicWallElements, parametersToCreate);

                //ErrorMessage = "Unviable Setup: Cannot find RB fields";
                //return false;
            }

            if (!hasStructWalls)
            {
                ErrorMessage = "Unviable Setup: Cannot find wall assemblies.";
                return false;
            }

            // Remove walls that do not have mark field
            List<Element> basicWallElementsCopy = new List<Element>(basicWallElements);
            while (basicWallElementsCopy.Count > 0)
            {
                Element wall = basicWallElementsCopy.First();
                string wallMark = GetParameterAsString(wall, "Mark");
                if (String.IsNullOrEmpty(wallMark))
                    basicWallElements.Remove(wall);

                basicWallElementsCopy.Remove(wall);
            }

            // Check if Basic walls have dimensions
            bool hasDimensions = true;

            // Ensures that if atleast one panel does not have dimensions,
            // hasDimensions will be false
            foreach (Element element in basicWallElements)
                if (GetParameterAsDouble(element, "Assembly Area") == 0)
                    hasDimensions = false;

            if (!hasDimensions)
            {
                // Good chance that dimensions have not been created
                ErrorMessage = "Unviable Setup: Cannot find dimensions. Calculate assembly dimensions and try again";
                return false;
            }

            // Get Basic wall dimensions and create panel
            foreach (Element basicWall in basicWallElements)
            {
                // Panel to add
                Panel panel = null;

                // Find Struct wall that matches Basic wall
                string basicWallName = GetParameterAsString(basicWall, "Mark");
                bool hasMatch = false;
                Element structWallMatch = null;

                if (hasStructWalls)
                    foreach (Element structWall in structWallElements)
                        if (structWall.Name.Equals(basicWallName))
                        {
                            structWallMatch = structWall;
                            hasMatch = true;
                        }
                
                // Create the panel
                panel = new Panel(basicWall, structWallMatch);


                // Get panel type
                // 1. Get the name of type of this panel
                // 2. if the job does not already have type of this panel, add it
                // 
                panel.Type = new Type(panel.Name.FullName);

                // Fill in fields for panel
                foreach (string paramName in ParamNames)
                {
                    double paramValue = 0;
                    if (paramName.Equals("Framing Member Mass"))
                        paramValue = GetParameterAsDouble(structWallMatch, paramName);
                    else
                        paramValue = GetParameterAsDouble(basicWall, paramName);

                    if (paramValue == 0)
                    {
                        // Does not have parameter value
                        ErrorMessage = String.Format("Unviable Setup: Cannot find parameter {0} in panel {1}.", paramName, panel.Name);
                        return false;
                    }

                    switch (paramName)
                    {
                        case "Assembly Height":
                            panel.Height = new Height(Math.Round(paramValue * 12, 3));
                            break;
                        case "Assembly Length":
                            panel.Width = new Width(Math.Round(paramValue * 12, 3));
                            break;
                        case "Assembly Depth":
                            panel.Plate = new Plate(Math.Round(paramValue * 12, 3), panel.Type.Name);
                            break;
                        case "Assembly Area":
                            panel.Area = Math.Round(paramValue);
                            break;
                        case "Framing Member Mass":
                            panel.Weight = Math.Round(paramValue);
                            break;
                    }
                }

                // Get mass parameter is there is a struct wall
                if (hasMatch)
                {
                    double paramValue = GetParameterAsDouble(panel.StructWall, "Framing Member Mass");

                    if (paramValue == 0)
                    {
                        // Does not have parameter value
                        ErrorMessage = String.Format("Unviable Setup: Cannot find parameter Framing Member Mass in panel {0}.", panel.Name);
                        return false;
                    }
                }

                // Switch width and height if the panel is sideways
                if (panel.Width.AsDouble > panel.Height.AsDouble)
                {
                    double temp = panel.Width.AsDouble;
                    panel.Width = new Width(panel.Height.AsDouble);
                    panel.Height = new Height(temp);
                }

                if (panel.Plate.Description == null)
                {
                    // Plate warning
                    ErrorMessage = String.Format("Unviable Setup: Plate is extremely large on panel {0}.", panel.Name.FullName);
                    return false;
                }

                panels.Add(panel);
            }

            // Sort panels by name
            panels = PanelNameSort.Sort(panels);

            // Set panels in project
            Project.Panels = panels;

            return true;
        }

        private static void CreateSharedParameters(List<Element> wallElements, List<string> parameterNames)
        {
            Category category = ProjectState.Doc.Settings.Categories.get_Item(BuiltInCategory.OST_Walls);
            CategorySet categorySet = ProjectState.App.Create.NewCategorySet();
            categorySet.Insert(category);

            string originalFile = ProjectState.App.SharedParametersFilename;
            string tempFile = @"C:\ProgramData\RedBuilt\RBRevit\RBRevit_Temp_Bundle_Parameters.txt";

            using (File.Create(tempFile)) { }

            try
            {
                ProjectState.App.SharedParametersFilename = tempFile;

                DefinitionFile sharedParametersFile = ProjectState.App.OpenSharedParameterFile();

                DefinitionGroup rbGroup = sharedParametersFile.Groups.Create("RB");

                // Bind to all generic wall objects
                InstanceBinding instanceBinding = ProjectState.App.Create.NewInstanceBinding(categorySet);

                Element element = wallElements[0];

                using (Transaction transaction = new Transaction(ProjectState.Doc))
                {
                    transaction.Start("Add Shared Parameters");

                    foreach (string parameterName in parameterNames)
                    {
                        // Definition options
                        ExternalDefinitionCreationOptions options = new ExternalDefinitionCreationOptions(parameterName, ParameterType.Text);

                        // Create definition
                        ExternalDefinition externalDefinition = rbGroup.Definitions.Create(options) as ExternalDefinition;

                        // Insert parameters
                        ProjectState.Doc.ParameterBindings.Insert(externalDefinition, instanceBinding);
                    }

                    transaction.Commit();
                }
            }
            catch { }
            finally
            {
                ProjectState.App.SharedParametersFilename = originalFile;
                File.Delete(tempFile);
            }
        }

        /// <summary>
        /// Checks the list of panels to ensure that there are no other panels 
        /// with the same name as panel
        /// </summary>
        /// <param name="panel">the panel to ensure does not exist twice</param>
        /// <param name="panels">list of panels</param>
        /// <returns>true if there are two panels with the same name, false otherwise</returns>
        private static bool HasDuplicatePanel(Panel panel, List<Panel> panels)
        {
            return true;
        }

        /// <summary>
        /// Imports the project data including project number, name, and location
        /// </summary>
        /// <param name="doc">revit document</param>
        /// <returns>true if import success, false otherwise</returns>
        private static bool GetProject(Document doc)
        {
            List<Element> viewSheetSet = new FilteredElementCollector(doc).OfClass(typeof(ViewSheet)).ToList();
            ViewSheet projectCoverPage = (ViewSheet)viewSheetSet.First(x => x.Name == "Project Cover Page");

            // Project Number
            if (doc.ProjectInformation.Number == null || doc.ProjectInformation.Number.Trim(' ').Length == 0)
            {
                ErrorMessage = "Unviable Setup: Cannot find Project Number.";
                return false;
            }

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
            {
                ErrorMessage = "Unviable Setup: Cannot find Project Name.";
                return false;
            }

            // Project Location
            if (doc.ProjectInformation.Address == null)
            {
                ErrorMessage = "Unviable Setup: Cannot find Project Location.";
                return false;
            }
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

            return true;
        }

        private static string GetParameterAsString(Element element, string param)
        {
            return element.LookupParameter(param).AsString();
        }

        private static double GetParameterAsDouble(Element element, string param)
        {
            return element.LookupParameter(param).AsDouble();
        }
    }
}
