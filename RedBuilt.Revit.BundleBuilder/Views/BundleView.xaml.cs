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
using RedBuilt.Revit.BundleBuilder.Data.Models;
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
        }
    }
}
