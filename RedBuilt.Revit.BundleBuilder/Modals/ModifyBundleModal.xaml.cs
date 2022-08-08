using RedBuilt.Revit.BundleBuilder.Data.Models;
using RedBuilt.Revit.BundleBuilder.Data.Services;
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
using System.Windows.Shapes;

namespace RedBuilt.Revit.BundleBuilder.Modals
{
    /// <summary>
    /// Interaction logic for ModifyBundleModal.xaml
    /// </summary>
    public partial class ModifyBundleModal : Window
    {
        public ModifyBundleModal()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(this.BundleLocation.Text, out int destBundleNumber) &&
                Int32.TryParse(this.Bundles.Text, out int bundleNumber))
                DataService.ProcessModification(Project.Bundles.Where(x => x.Number == bundleNumber).FirstOrDefault(), destBundleNumber);
            else
            {
                BundleLocation.Text = "";
                MessageBox.Show("Invalid Move");
            }

            // Close the popup
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the popup
            this.Close();
        }
    }
}
