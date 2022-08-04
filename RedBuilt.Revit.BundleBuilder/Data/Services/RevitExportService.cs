using Autodesk.Revit.DB;
using RedBuilt.Revit.BundleBuilder.Data.Models;
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
        /// FM parameter names and thier guids 
        /// </summary>
        private static readonly Dictionary<string, Guid> ParameterNameAndGuid = new Dictionary<string, Guid>
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
            using (Transaction transaction = new Transaction(doc, "BundleBuilder"))
            {
                // Start the Transaction
                transaction.Start();

                foreach (Panel panel in Project.Panels)
                {
                    #region RB Fields

                    Element wallElement = panel.WallElement;

                    // Change RB Bundle parameter to panel bundle number
                    wallElement.LookupParameter("RB Bundle")?.Set(panel.Bundle.Number.ToString());

                    // Change RB Bundle Level parameter to panel level
                    wallElement.LookupParameter("RB Bundle Level")?.Set(panel.Level.Number.ToString());

                    // Change RB Bundle Column parameter to panel column
                    wallElement.LookupParameter("RB Bundle Column")?.Set(panel.Column.ToString());

                    // Change RB Bundle Depth parameter to panel depth
                    wallElement.LookupParameter("RB Bundle Depth")?.Set(panel.Depth.ToString());

                    #endregion

                    #region FM TR Fields

                    Element panelElement = panel.PanelElement;

                    // Change FM TR Number parameter to panel bundle number
                    panelElement.get_Parameter(ParameterNameAndGuid["FM TR Number"]).Set(panel.Bundle.Number.ToString());

                    // Change FM TR Column Number parameter to panel level index
                    panelElement.get_Parameter(ParameterNameAndGuid["FM TR Column Number"]).Set((panel.Level.Panels.IndexOf(panel) + 1).ToString());

                    // Change FM TR Row Number parameter to panel level number
                    panelElement.get_Parameter(ParameterNameAndGuid["FM TR Row Number"]).Set(panel.Level.Number.ToString());

                    // Change FM TR Type parameter to "Bundle"
                    panelElement.get_Parameter(ParameterNameAndGuid["FM TR Type"]).Set("Bundle");

                    // Change Comments parameter to panel bundle number
                    panelElement.LookupParameter("Comments").Set(panel.Bundle.Number.ToString());

                    #endregion
                }

                // Commit all changes on the transaction
                transaction.Commit();
                // The transaction is disposed
            }
        }
    }
}
