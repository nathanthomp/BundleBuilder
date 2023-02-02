using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RedBuilt.Revit.BundleBuilder
{
    public enum CommandKind
    {
        BUNDLE,
        VERSION
    }

    [Transaction(TransactionMode.Manual)]
    public class BundleCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                return CommandHelper.Process(commandData, ref message, elements, CommandKind.BUNDLE);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class VersionCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                return CommandHelper.Process(commandData, ref message, elements, CommandKind.VERSION);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }

    public class CommandHelper
    {
        private static Document s_doc;

        public static Result Process(ExternalCommandData commandData, ref string message, ElementSet elements, CommandKind commandKind)
        {
            s_doc = commandData.Application.ActiveUIDocument.Document;

            if (!CommandHelper.IsVersionCompatible(commandData))
            {
                message = "BundleBuilder is not supported for your version of Revit.";
                return Result.Cancelled;
            }

            // REMINDER: this is where to make operations for every command type

            switch (commandKind)
            {
                case CommandKind.BUNDLE:
                    return ProcessBundle(commandData, ref message, elements);
                case CommandKind.VERSION:
                    return ProcessVersion(commandData, ref message, elements);
                default: 
                    return Result.Cancelled;
            }
        }

        public static Result ProcessBundle(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            message = "Not implemented";
            return Result.Failed;
        }

        public static Result ProcessVersion(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                var version = typeof(ExternalApplication).Assembly.GetName().Version;
                MessageBox.Show("BundleBuilder Version: " + version);
            } 
            catch
            {
                MessageBox.Show("BundleBuilder Version: x.x.x.x");
            }
            return Result.Succeeded;
        }

        public static bool IsVersionCompatible(ExternalCommandData commandData)
        {
            return (
                    commandData.Application.Application.VersionNumber.Contains("2022") ||
                    commandData.Application.Application.VersionNumber.Contains("2023")
                   );
        }
    }
}
