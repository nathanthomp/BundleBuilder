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
    /// Interaction logic for ModifyModal.xaml
    /// </summary>
    public partial class ModifyModal : Window
    {
        private static List<string> panelNames;
        private static List<string> levelNames;

        public ModifyModal()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            bool panelChecked = (bool)panelCheckBox.IsChecked;
            bool levelChecked = (bool)levelCheckBox.IsChecked;
            bool newLevel = (bool)newLevelCheckBox.IsChecked;

            string moveOption = "";

            if (panelChecked)
                moveOption = "panel";
            if (levelChecked)
                moveOption = "level";

            string moveObject = dataComboBox.Text;

            if (Int32.TryParse(bundleTextBox.Text, out int bundleDest) &&
                Int32.TryParse(levelTextBox.Text, out int levelDest))
                if (ModifyService.ProcessModification(moveOption, moveObject, bundleDest, levelDest, newLevel))
                    MessageBox.Show("Move Successful");
                else
                    MessageBox.Show(ModifyService.ErrorMessage);

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the popup
            this.Close();
        }

        private void panelCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (panelNames == null)
            {
                List<string> panels = new List<string>();
                foreach (Panel panel in Project.Panels)
                    panels.Add(panel.Name.FullName);

                panelNames = panels;
            }

            dataComboBox.ItemsSource = panelNames;
            levelCheckBox.IsEnabled = false;
            newLevelCheckBox.IsEnabled = true;
        }

        private void levelCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (levelNames == null)
            {
                List<string> levels = new List<string>();
                foreach (Bundle bundle in Project.Bundles)
                    for (int i = bundle.Levels.Count; i > 0; i--)
                        levels.Add(bundle.Levels.Where(x => x.Number == i).First().ToString());

                levelNames = levels;
            }

            dataComboBox.ItemsSource = levelNames;

            panelCheckBox.IsEnabled = false;
            newLevelCheckBox.IsEnabled = false;
        }

        private void panelCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            levelCheckBox.IsEnabled = true;

        }

        private void levelCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            panelCheckBox.IsEnabled = true;
        }
    }
}
