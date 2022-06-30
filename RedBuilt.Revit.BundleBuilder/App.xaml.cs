using RedBuilt.Revit.BundleBuilder.ViewModels;
using RedBuilt.Revit.BundleBuilder.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;


namespace RedBuilt.Revit.BundleBuilder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //
        // Might be a good spot for Revit Execute method
        //

        //
        // A constructor here would give the ability to implement DI
        //

        /// <summary>
        /// Entry point for application
        /// </summary>
        /// <param name="e">startup event data</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            NavigationState navigationState = new NavigationState();

            navigationState.CurrentViewModel = new BundleViewModel(navigationState);

            MainWindow window = new MainWindow
            {
                DataContext = new MainWindowViewModel(navigationState)
            };

            window.Show();

            base.OnStartup(e);
        }
    }
}
