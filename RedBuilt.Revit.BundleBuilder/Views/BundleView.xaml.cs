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
        }

        /// <summary>
        /// Changes this panel ToBundle property to false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // get panel of checkbox
            // change that panel property to false
        }

        /// <summary>
        /// Changes this panel ToBundle property to true
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            string panelName = "";
            System.Windows.Controls.CheckBox checkBox = sender as System.Windows.Controls.CheckBox;
            WrapPanel wrapPanel = (WrapPanel)checkBox.Parent;
            TextBlock _TextBlock = new TextBlock();
            foreach (var child in wrapPanel.Children)
            {
                if (child.GetType().ToString() == "System.Windows.Controls.TextBlock")
                {
                    _TextBlock = (TextBlock)child;
                    panelName = _TextBlock.Text;
                }
            }
            foreach (Panel panel in Panels)
            {
                if (panel.Name == panelName)
                {
                    panel.ToBundle = true;
                    break;
                }
            }
        }
    }
}
