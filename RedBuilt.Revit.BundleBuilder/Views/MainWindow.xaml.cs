using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
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
    [Transaction(TransactionMode.Manual)]
    public class RevitCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = uiApp.Application;
            Document doc = uiDoc.Document;
            try
            {
                MainWindow mw = new MainWindow(doc);
                mw.ShowDialog();
                return Result.Succeeded;
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                TaskDialog.Show("BundleBuilder", "This operation was cancelled");
                return Result.Cancelled;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("BundleBuilder", "The following error occured: " + ex.Message);
                return Result.Failed;
            }
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Document _doc;
        public MainWindow(Document doc)
        {
            NavigationState navigationState = new NavigationState();
            navigationState.CurrentViewModel = new BundleViewModel(navigationState);

            DataContext = new MainWindowViewModel(navigationState);

            _doc = doc;

            InitializeComponent();

        }
    }
}
