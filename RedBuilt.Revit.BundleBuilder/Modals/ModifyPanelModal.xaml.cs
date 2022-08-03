using RedBuilt.Revit.BundleBuilder.Application.Tools;
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
using Panel = RedBuilt.Revit.BundleBuilder.Data.Models.Panel;

namespace RedBuilt.Revit.BundleBuilder.Modals
{
    /// <summary>
    /// Interaction logic for ModifyPanelModal.xaml
    /// </summary>
    public partial class ModifyPanelModal : Window
    {
        public ModifyPanelModal()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            if (Int32.TryParse(this.BundleLocation.Text, out int destBundleNumber) &&
                Int32.TryParse(this.LevelLocation.Text, out int destLevelNumber) &&
                DataIsValid(destBundleNumber, destLevelNumber))
            {
                // Get Level to move
                Panel panel = PanelTools.GetPanelFromName(this.Panels.Text);

                // Process the requested modification
                DataService.ProcessModification(panel, destBundleNumber, destLevelNumber);

                // Close the popup
                this.Close();
            }
            else
            {
                this.BundleLocation.Text = "";
                this.LevelLocation.Text = "";
                this.Panels.Text = "";

                MessageBox.Show("Invalid Move");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the popup
            this.Close();
        }

        private bool DataIsValid(int destBundleNumber, int destLevelNumber)
        {
            bool result = true;

            Bundle bundle = Project.Bundles.Where(x => x.Number == destBundleNumber).First();

            if (destLevelNumber < 1 || destLevelNumber > bundle.NumberOfLevels + 1)
                result = false;

            if (destBundleNumber < 1 || destBundleNumber > Project.Bundles.Count + 1)
                result = false;

            return result;
        }
    }
}
