using RedBuilt.Revit.BundleBuilder.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.Commands
{
    public class NavigateCommand<T> : Command
        where T : ViewModel
    {
        private readonly NavigationState _navigationState;
        private readonly Func<T> _createViewModel;

        public NavigateCommand(NavigationState navigationState, Func<T> createViewModel)
        {
            _navigationState = navigationState;
            _createViewModel = createViewModel;
        }

        public override void Execute(object parameter)
        {
            _navigationState.CurrentViewModel = _createViewModel();
        }
    }
}
