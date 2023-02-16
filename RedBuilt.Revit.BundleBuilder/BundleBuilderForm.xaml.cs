using RedBuilt.Revit.BundleBuilder.Components;
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

namespace RedBuilt.Revit.BundleBuilder
{
    /// <summary>
    /// Interaction logic for BundleBuilderForm.xaml
    /// </summary>
    public partial class BundleBuilderForm : Window
    {
        public Project Project { get; private set; }

        public BundleBuilderForm(Project project)
        {
            InitializeComponent();
            this.Project = project;
        }
    }
}
