using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        private NavigationState _navigationState;

        public ViewModel CurrentViewModel => _navigationState.CurrentViewModel;

        public MainWindowViewModel(NavigationState navigationState)
        {
            _navigationState = navigationState;
            _navigationState.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            NotifyPropertyChanged(nameof(CurrentViewModel));
        }











    }
}
