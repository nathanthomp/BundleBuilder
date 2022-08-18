using Autodesk.Revit.DB;
using RedBuilt.Revit.BundleBuilder.Data.Models;
using RedBuilt.Revit.BundleBuilder.Data.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Panel = RedBuilt.Revit.BundleBuilder.Data.Models.Panel;

namespace RedBuilt.Revit.BundleBuilder.Data.Services
{
    public static class RevitExportService
    {
        /// <summary>
        /// Names of RB fields
        /// </summary>
        private static List<string> RBFieldNames = new List<string>()
        {
            "RB Bundle",
            "RB Bundle Level",
            "RB Bundle Column",
            "RB Bundle Depth"
        };

        /// <summary>
        /// FM parameter names and thier guids 
        /// </summary>
        private static readonly Dictionary<string, Guid> FMParameterNamesAndGuid = new Dictionary<string, Guid>
        {
            { "FM TR Number", new Guid("8d03c405-03b0-4f19-b18d-da8f23c5fd0f")},
            { "FM TR Column Number", new Guid("602730bd-59da-47eb-a04f-819262d6eac4") },
            { "FM TR Row Number", new Guid("1ec33ab7-867b-4663-ba4c-e6e50f0cd592") },
            { "FM TR Type", new Guid("8ccd8cec-1a6d-4631-8cfa-8e9b4b2e9a56") }
        };

        /// <summary>
        /// Exports data to Revit RB and FM Fields
        /// </summary>
        /// <param name="doc">revit document</param>
        public static void Export(Document doc)
        {
            // Determine whether there are RB Fields, and if not, create them

            using (Transaction transaction = new Transaction(doc, "BundleBuilder"))
            {
                // Start the Transaction
                transaction.Start();

                foreach (Panel panel in Project.Panels)
                {
                    #region RB Fields

                    Element basicWall = panel.BasicWall;

                    // Change RB Bundle parameter to panel bundle number
                    basicWall.LookupParameter("RB Bundle")?.Set(panel.Bundle.Number.ToString());

                    // Change RB Bundle Level parameter to panel level
                    basicWall.LookupParameter("RB Bundle Level")?.Set(panel.Level.Number.ToString());

                    // Change RB Bundle Column parameter to panel column
                    basicWall.LookupParameter("RB Bundle Column")?.Set(panel.Column.ToString());

                    // Change RB Bundle Depth parameter to panel depth
                    basicWall.LookupParameter("RB Bundle Depth")?.Set(panel.Depth.ToString());

                    #endregion

                    #region FM TR Fields

                    Element structWall = panel.StructWall;

                    if (structWall != null)
                    {
                        // Change FM TR Number parameter to panel bundle number
                        structWall.get_Parameter(FMParameterNamesAndGuid["FM TR Number"]).Set(panel.Bundle.Number.ToString());

                        // Change FM TR Column Number parameter to panel level index
                        structWall.get_Parameter(FMParameterNamesAndGuid["FM TR Column Number"]).Set((panel.Level.Panels.IndexOf(panel) + 1).ToString());

                        // Change FM TR Row Number parameter to panel level number
                        structWall.get_Parameter(FMParameterNamesAndGuid["FM TR Row Number"]).Set(panel.Level.Number.ToString());

                        // Change FM TR Type parameter to "Bundle"
                        structWall.get_Parameter(FMParameterNamesAndGuid["FM TR Type"]).Set("Bundle");

                        // Change Comments parameter to panel bundle number
                        structWall.LookupParameter("Comments").Set(panel.Bundle.Number.ToString());
                    }

                    #endregion
                }

                // Commit all changes on the transaction
                transaction.Commit();
                // The transaction is disposed
            }
        }

        //public static bool CreateParameterBindings()
        //{
        //    // Determine if the wall object has RB parameters
        //    if (Project.Panels[0].BasicWall.LookupParameter("RB Bundle") == null)
        //    {
        //        try
        //        {
        //            ProjectState.App.SharedParametersFilename = @"C:\ProgramData\RedBuilt\RBRevit\RBRevit_Master_Shared_Parameters.txt";

        //            // Get access to definition file
        //            DefinitionFile definitionFile = ProjectState.App.OpenSharedParameterFile();

        //            // Create group in the shared parameters file
        //            DefinitionGroup definitionGroup = definitionFile.Groups.Where(x => x.Name.Equals("RB Data")).First();

        //            // Create catergory set and add walls to it
        //            CategorySet categorySet = ProjectState.UIApp.Application.Create.NewCategorySet();
        //            Category category = Category.GetCategory(ProjectState.Doc, BuiltInCategory.OST_Walls);
        //            categorySet.Insert(category);

        //            // Create instance of instance binding
        //            InstanceBinding instanceBinding = ProjectState.UIApp.Application.Create.NewInstanceBinding(categorySet);

        //            foreach (string name in RBFieldNames)
        //            {
        //                // Create instance definitions in definition group RBFields
        //                ExternalDefinitionCreationOptions externalDefinitionCreationOptions = new ExternalDefinitionCreationOptions(name, ParameterType.Integer);
        //                Definition definition = definitionGroup.Definitions.Create(externalDefinitionCreationOptions);

        //                // Insert new parameter
        //                ProjectState.UIApp.ActiveUIDocument.Document.ParameterBindings.Insert(definition, instanceBinding);
        //            }


        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //    else
        //        return false;

        //}
    }
}
