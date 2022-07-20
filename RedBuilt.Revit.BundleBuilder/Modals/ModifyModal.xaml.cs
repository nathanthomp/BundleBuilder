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
            // When data entered is validated
            // Use DataService to add either: panel to level, level to bundle, bundle to project, or all
            // Use DataService to remove either: panel from level, level from bundle, bundle from project, or all

            // Assume that panel is not empty, level number is [1 , bundle.level.count + 1]

            // Get Level to move
            Level level = LevelTools.GetLevelFromName(this.Levels.Text);

            // Get bundle number Destination
            if (Int32.TryParse(this.BundleLocation.Text, out int destBundleNumber)) { }

            // Get level number
            if (Int32.TryParse(this.LevelLocation.Text, out int destLevelNumber)) { }

            // Process the resquested modification
            DataService.ProcessModification(level, destBundleNumber, destLevelNumber);

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
