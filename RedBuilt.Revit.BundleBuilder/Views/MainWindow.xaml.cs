using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RedBuilt.Revit.BundleBuilder.Data.Models;
using RedBuilt.Revit.BundleBuilder.Data.Services;
using RedBuilt.Revit.BundleBuilder.Data.States;
using RedBuilt.Revit.BundleBuilder.ViewModels;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(Document doc)
        {
            ProjectState.Doc = doc;
            ProjectState.MainWindow = this;

            NavigationState navigationState = new NavigationState();
            BundleViewModel bundleViewModel = new BundleViewModel(navigationState);
            navigationState.CurrentViewModel = bundleViewModel;
            ProjectState.BundleViewModelInstanciated = true;

            DataContext = new MainWindowViewModel(navigationState);

            Project.Panels = RevitImportService.GetPanels(ProjectState.Doc);
            RevitImportService.GetProject(ProjectState.Doc);

            InitializeComponent();
        }
    }
}
