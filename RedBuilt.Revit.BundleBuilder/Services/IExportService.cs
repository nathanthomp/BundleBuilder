using RedBuilt.Revit.BundleBuilder.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RedBuilt.Revit.BundleBuilder.Services
{
    public interface IExportService
    {
        void Export();

        void Restore();
    }
}
