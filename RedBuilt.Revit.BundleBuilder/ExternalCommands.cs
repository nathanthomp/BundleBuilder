using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RedBuilt.Revit.BundleBuilder.Data.Services;
using RedBuilt.Revit.BundleBuilder.Data.States;
using RedBuilt.Revit.BundleBuilder.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RedBuilt.Revit.BundleBuilder
{
    public enum CommandKind
    {
        BUNDLE,
        REVISION
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
                return CommandHelper.Process(commandData, ref message, elements, CommandKind.REVISION);
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
                case CommandKind.REVISION:
                    return ProcessRevision(commandData, ref message, elements);
                default: 
                    return Result.Cancelled;
            }
        }

        public static Result ProcessBundle(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            message = "Not implemented";
            return Result.Failed;
        }

        public static Result ProcessRevision(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            message = "Not implemented";
            return Result.Failed;
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
