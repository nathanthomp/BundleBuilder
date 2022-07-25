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
using RedBuilt.Revit.BundleBuilder.Application.Tools;
using RedBuilt.Revit.BundleBuilder.Data.Models;
using RedBuilt.Revit.BundleBuilder.Data.Services;
using RedBuilt.Revit.BundleBuilder.Data.States;
using RedBuilt.Revit.BundleBuilder.Views;
using Panel = RedBuilt.Revit.BundleBuilder.Data.Models.Panel;

namespace RedBuilt.Revit.BundleBuilder.Modals
{
    /// <summary>
    /// Interaction logic for ModifyModal.xaml
    /// </summary>
    public partial class ModifyModal : Window
    {
        public ModifyModal()
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
                Level level = LevelTools.GetLevelFromName(this.Levels.Text);

                // Process the resquested modification
                DataService.ProcessModification(level, destBundleNumber, destLevelNumber);

                // Close the popup
                this.Close();
            }
            else
            {
                this.BundleLocation.Text = "";
                this.LevelLocation.Text = "";
                this.Levels.Text = "";

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

            if (destLevelNumber < 1 ||  destLevelNumber > bundle.NumberOfLevels + 1)
                result = false;

            if (destBundleNumber < 1 || destBundleNumber > Project.Bundles.Count + 1)
                result = false;

            return result;
        }
    }
}
