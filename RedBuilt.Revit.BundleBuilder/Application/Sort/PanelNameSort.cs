using RedBuilt.Revit.BundleBuilder.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.Application.Sort
{
    public class PanelNameSort
    {
        public static List<Panel> Sort(List<Panel> panels)
        {
            return panels.OrderBy(x => x.Name).ToList();
        } 


    }
}
