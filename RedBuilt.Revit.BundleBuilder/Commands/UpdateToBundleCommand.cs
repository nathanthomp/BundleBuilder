using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RedBuilt.Revit.BundleBuilder.Commands
{
    public class UpdateToBundleCommand : Command
    {
        public override void Execute(object parameter)
        {
            MessageBox.Show("Updated!");
        }
    }
}
