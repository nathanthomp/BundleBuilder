using RedBuilt.Revit.BundleBuilder.Data.Models;
using RedBuilt.Revit.BundleBuilder.Data.States;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace RedBuilt.Revit.BundleBuilder.Commands
{
    public class ExportCommand : Command
    {
        public override void Execute(object parameter)
        {
            // Export bundle data to revit
            Data.Services.RevitExportService.Export(ProjectState.Doc);

            // Choose folder location to save bundle prints
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string fileName = fbd.SelectedPath + "\\" + Project.Number + "_BundlePrints.pdf";
                    Application.Reports.BundlePrintsReport.CreatePdf(fileName);
                }
            }

            // Show proof of export
            System.Windows.MessageBox.Show("Success!");

            // Close application
            ProjectState.MainWindow.Close();
        }
    }
}
