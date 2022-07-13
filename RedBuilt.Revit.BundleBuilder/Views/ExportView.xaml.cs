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

namespace RedBuilt.Revit.BundleBuilder.Views
{
    /// <summary>
    /// Interaction logic for ExportView.xaml
    /// </summary>
    public partial class ExportView : UserControl
    {
        public ExportView()
        {
            ProjectState.ExportView = this;
            InitializeComponent();
        }

        private void CreatedBundles_Loaded(object sender, RoutedEventArgs e)
        {
            createdBundles.Navigate(new Uri(@"C:\RedBuilt\Revit\BundleBuilder\RedBuilt.Revit.BundleBuilder\Documents\BundleReport.html"));
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            ModifyModal mm = new ModifyModal();
            mm.Panels.ItemsSource = Project.Panels;
            mm.ShowDialog();
        }

        private void ViewOnWeb_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\RedBuilt\Revit\BundleBuilder\RedBuilt.Revit.BundleBuilder\Documents\BundleReport.html");
        }
    }
}
