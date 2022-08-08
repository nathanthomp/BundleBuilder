using RedBuilt.Revit.BundleBuilder.Data.Models;
using RedBuilt.Revit.BundleBuilder.Data.States;
using RedBuilt.Revit.BundleBuilder.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Panel = RedBuilt.Revit.BundleBuilder.Data.Models.Panel;

namespace RedBuilt.Revit.BundleBuilder.Views
{
    /// <summary>
    /// Interaction logic for ExportView.xaml
    /// </summary>
    public partial class ExportView : UserControl
    {
        private static readonly string filePath = @"C:\RedBuilt\Revit\BundleBuilder\RedBuilt.Revit.BundleBuilder\Documents\BundleReport.html";

        public ExportView()
        {
            ProjectState.ExportView = this;
            InitializeComponent();
        }

        private void CreatedBundles_Loaded(object sender, RoutedEventArgs e)
        {
            createdBundles.Navigate(new Uri(filePath));
        }

        private void ModifyBundle_Click(object sender, RoutedEventArgs e)
        {
            ModifyBundleModal mbm = new ModifyBundleModal();

            // Create list of bundles
            List<int> bundles = new List<int>();
            foreach (Bundle bundle in Project.Bundles)
                bundles.Add(bundle.Number);

            mbm.Bundles.ItemsSource = bundles;
            mbm.ShowDialog();
        }

        private void ModifyPanel_Click(object sender, RoutedEventArgs e)
        {
            ModifyPanelModal mpm = new ModifyPanelModal();

            // Create list of panels in project
            List<string> panels = new List<string>();
            foreach (Panel panel in Project.Panels)
                panels.Add(panel.ToString());

            mpm.Panels.ItemsSource = panels;
            mpm.ShowDialog();
        }

        private void ModifyLevel_Click(object sender, RoutedEventArgs e)
        {
            ModifyLevelModal mlm = new ModifyLevelModal();

            // Create list of levels in project
            List<string> levels = new List<string>();
            foreach (Bundle bundle in Project.Bundles)
                for (int i = bundle.Levels.Count; i > 0; i--)
                    levels.Add(bundle.Levels.Where(x => x.Number == i).First().ToString());

            mlm.Levels.ItemsSource = levels;
            mlm.ShowDialog();
        }

        private void ViewOnWeb_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(filePath);
        }


    }
}
