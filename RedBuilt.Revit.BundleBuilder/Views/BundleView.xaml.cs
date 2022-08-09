using System;
using System.Collections.Generic;
using System.Globalization;
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
using RedBuilt.Revit.BundleBuilder.Data.States;
using RedBuilt.Revit.BundleBuilder.ViewModels;
using Panel = RedBuilt.Revit.BundleBuilder.Data.Models.Panel;

namespace RedBuilt.Revit.BundleBuilder.Views
{
    /// <summary>
    /// Interaction logic for BundleView.xaml
    /// </summary>
    public partial class BundleView : UserControl
    {
        public BundleView()
        {
            InitializeComponent();

            // Assign data to xaml element
            panels.ItemsSource = Project.Panels;

            // Assign data to collection view source
            if (ProjectState.BundleViewModelInstanciated)
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(panels.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("Type.Name");
                view.GroupDescriptions.Add(groupDescription);

                ProjectState.BundleViewModelInstanciated = false;
            }
        }

        private void OnGroup_Checked(object sender, RoutedEventArgs e)
        {
            // Get the type name of group that was checked
            string nameOfGroupChecked = "";
            CheckBox checkBox = (CheckBox)sender;
            StackPanel stackPanel = (StackPanel)checkBox.Parent;

            foreach (var child in stackPanel.Children)
            {
                if (child.ToString().Equals("System.Windows.Controls.TextBlock"))
                {
                    TextBlock textBlock = (TextBlock)child;
                    if (textBlock.Name.Equals("typeName"))
                        nameOfGroupChecked = textBlock.Text;
                }
            }

            if (!String.IsNullOrEmpty(nameOfGroupChecked))
            {
                // Get panels that are of that type and change their ToBundle to true
                foreach (Panel panel in Project.Panels)
                    if (panel.Type.Name.Equals(nameOfGroupChecked))
                        panel.ToBundle = true;
            }
        }

        private void OnGroup_UnChecked(object sender, RoutedEventArgs e)
        {
            // Get the type name of group that was checked
            string nameOfGroupChecked = "";
            CheckBox checkBox = (CheckBox)sender;
            StackPanel stackPanel = (StackPanel)checkBox.Parent;

            foreach (var child in stackPanel.Children)
            {
                if (child.ToString().Equals("System.Windows.Controls.TextBlock"))
                {
                    TextBlock textBlock = (TextBlock)child;
                    if (textBlock.Name.Equals("typeName"))
                        nameOfGroupChecked = textBlock.Text;
                }
            }

            if (!String.IsNullOrEmpty(nameOfGroupChecked))
            {
                // Get panels that are of that type and change their ToBundle to true
                foreach (Panel panel in Project.Panels)
                    if (panel.Type.Name.Equals(nameOfGroupChecked))
                        panel.ToBundle = false;
            }
        }
    }
}
