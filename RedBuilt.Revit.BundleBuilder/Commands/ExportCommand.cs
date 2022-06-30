using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RedBuilt.Revit.BundleBuilder.Commands
{
    public class ExportCommand : Command
    {
        public override void Execute(object parameter)
        {
            //
            // This will be where the Data goes back into Revit 
            //
            MessageBox.Show("Exported!");
        }
    }
}
