using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPISumLenghtPipe
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> selectedElementRefList = uidoc.Selection.PickObjects(ObjectType.Element, new PipeFilter(), "Выберите трубы");
            var pipeList = new List<Pipe>(); 
            double lengthParam = 0;

            foreach (var selectedElement in selectedElementRefList)
            {
                Pipe oPipe = doc.GetElement(selectedElement) as Pipe;
                pipeList.Add(oPipe);
                lengthParam += oPipe.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble();
            }

            double length = UnitUtils.ConvertFromInternalUnits(lengthParam, UnitTypeId.Meters);
            TaskDialog.Show("Длина выбранных труб", $"Длина выбранных труб: {length} м.");

            return Result.Succeeded;
        }
    }
}
