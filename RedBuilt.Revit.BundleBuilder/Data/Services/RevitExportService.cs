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
        /// 
        /// </summary>
        private static readonly Dictionary<string, string> ParameterNameAndGuid = new Dictionary<string, string>
        {
            { "FM TR Number", "8d03c405-03b0-4f19-b18d-da8f23c5fd0f"},
            { "FM TR Column Number", "602730bd-59da-47eb-a04f-819262d6eac4" },
            { "FM TR Row Number", "1ec33ab7-867b-4663-ba4c-e6e50f0cd592" },
            { "FM TR Type", "8ccd8cec-1a6d-4631-8cfa-8e9b4b2e9a56" }
        };

        /// <summary>
        /// 
        /// </summary>
        public static void Export(Document doc)
        {
            using (Transaction transaction = new Transaction(doc, "BundleBuilder"))
            {
                // Start the Transaction
                transaction.Start();

                foreach (Panel panel in Project.Panels)
                {
                    // Get panel element and element parameter set
                    Element panelElement = panel.Element;
                    ParameterSet panelParameterSet = panelElement.Parameters;

                    // Change FM TR Number parameter to panel bundle number
                    panelElement.get_Parameter(new Guid("8d03c405-03b0-4f19-b18d-da8f23c5fd0f")).Set(panel.Bundle.Number.ToString());

                    // Change FM TR Column Number parameter to panel level index
                    panelElement.get_Parameter(new Guid("602730bd-59da-47eb-a04f-819262d6eac4")).Set((panel.Level.Panels.IndexOf(panel) + 1).ToString());

                    // Change FM TR Row Number parameter to panel level number
                    panelElement.get_Parameter(new Guid("1ec33ab7-867b-4663-ba4c-e6e50f0cd592")).Set(panel.Level.Number.ToString());

                    // Change FM TR Type parameter to "Bundle"
                    panelElement.get_Parameter(new Guid("8ccd8cec-1a6d-4631-8cfa-8e9b4b2e9a56")).Set("Bundle");

                    // Change Comments parameter to panel bundle number
                    panelElement.LookupParameter("Comments").Set(panel.Bundle.Number.ToString());
                }

                // Commit all changes on the transaction
                transaction.Commit();
                // The transaction is disposed
            }
        }
    }
}
