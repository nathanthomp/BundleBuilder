using Autodesk.Revit.DB;
using RedBuilt.Revit.BundleBuilder.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RedBuilt.Revit.BundleBuilder.Services
{
    public class RevitImportService
    {
        private class WallPair
        {
            public Element GeneralWallElement { get; set; }
            public Element StructuralFramingAssemblyElement { get; set; }
        }

        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectLocation { get; set; }
        public Queue<Components.Wall> Walls { get; set; }

        public RevitImportService(Document doc)
        {
            this.Import(doc);
        }

        private void Import(Document doc)
        {
            // NOTE: have users enter this info if not found
            this.ProjectId = !string.IsNullOrEmpty(doc.ProjectInformation.Number)
                ? doc.ProjectInformation.Number : throw new Exception("Could not find parameter Project Id in the Revit document");

            string name = doc.ProjectInformation.LookupParameter("Project Name")?.AsString();
            this.ProjectName = !string.IsNullOrEmpty(name)
                ? name : throw new Exception("Could not find parameter Project Name in the Revit document");

            this.ProjectLocation = !string.IsNullOrEmpty(doc.ProjectInformation.Address)
                ? doc.ProjectInformation.Address.Replace('\r', ' ').Replace('\n', ' ').Replace("  ", " ") : throw new Exception("Could not find parameter Project Location in the Revit document");

            List<Element> genericWallElements;
            Dictionary<string, Element> genericWallPairs;
            try
            {
                genericWallElements = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Walls)
                .OfClass(typeof(Autodesk.Revit.DB.Wall))
                .WhereElementIsNotElementType()
                .Where(element => !string.IsNullOrEmpty(element.LookupParameter("Mark").AsString()))
                .ToList();

                if (genericWallElements.Count() == 0)
                    throw new Exception("No eligible basic wall elements found");

                genericWallPairs = ToDictionary(genericWallElements);

            }
            catch (Exception ex)
            {
                throw new Exception("Could not find generic wall panel elements in the model:" + ex.Message);
            }
            
            List<Element> structuralFramingAssemblyElements;
            Dictionary<string, Element> structuralFramingAssemblyPairs;
            try
            {
                var elementId = new ElementId(BuiltInParameter.ELEM_FAMILY_PARAM);
                var pvp = new ParameterValueProvider(elementId);
                var fsrv = new FilterStringEquals();
                var fr = new FilterStringRule(pvp, fsrv, "Structural Framing Assembly", true);
                var paramFilter = new ElementParameterFilter(fr, false);
                structuralFramingAssemblyElements = new FilteredElementCollector(doc)
                    .OfCategory(BuiltInCategory.OST_Assemblies)
                    .WhereElementIsNotElementType()
                    .WherePasses(paramFilter)
                    .Where(element => !string.IsNullOrEmpty(element.LookupParameter("Mark").AsString()))
                    .ToList();

                if (structuralFramingAssemblyElements.Count() == 0)
                    throw new Exception("No eligible structural framing assemblies found");
                
                structuralFramingAssemblyPairs = ToDictionary(genericWallElements);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not find structural framing assembly elements in the model:" + ex.Message);
            }

            // Merge Dictionaries
            Dictionary<string, WallPair> walls = MergeDictionaries(genericWallPairs, structuralFramingAssemblyPairs);

            // Create walls
            foreach(KeyValuePair<string, WallPair> wall in walls)
            {

            }

        }

        private static Dictionary<string, Element> ToDictionary(List<Element> elements)
        {
            var result = new Dictionary<string, Element>();
            foreach (Element e in elements)
            {
                var name = e.LookupParameter("Mark").AsString();
                if (!result.ContainsKey(name))
                {
                    result.Add(name, e);
                }
                else
                {
                    throw new Exception("Duplicate name found for wall: " + name);
                }
            }
            return result;
        }

        private static Dictionary<string, WallPair> MergeDictionaries(Dictionary<string, Element> dict1, Dictionary<string, Element> dict2)
        {
            foreach (KeyValuePair<string, Element> pair1 in dict1)
            {
                string wallName = pair1.Key;
                if (dict2.ContainsKey(wallName))
                {

                }
                else
                {
                    throw new Exception("");
                }
            }
        }

        // Steps: 
        //      Get the project data
        //      Get both generic and assembly walls
        //      Make sure there are no duplicates in single list
        //      Make sure both lists have the same elements
        //
        //


    }
}
