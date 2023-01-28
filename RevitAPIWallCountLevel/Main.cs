using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPIWallCountLevel
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            ElementCategoryFilter elementWell = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
            ElementId level1Id = new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .Where(x => x.Name.Equals("Уровень 1"))
                .FirstOrDefault().Id;

            ElementId level2Id = new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .Where(x => x.Name.Equals("Уровень 2"))
                .FirstOrDefault().Id;


            ElementLevelFilter elementLevel1 = new ElementLevelFilter(level1Id);
            LogicalAndFilter wallFilter1 = new LogicalAndFilter(elementWell, elementLevel1);

            ElementLevelFilter elementLeve2 = new ElementLevelFilter(level2Id);
            LogicalAndFilter wallFilter2 = new LogicalAndFilter(elementWell, elementLeve2);

            var walls1 = new FilteredElementCollector(doc)
                .WherePasses(wallFilter1)
                .Cast<Wall>()
                .ToList();
            
            var walls2 = new FilteredElementCollector(doc)
                .WherePasses(wallFilter2)
                .Cast<Wall>()
                .ToList();
            
            TaskDialog.Show("Wall info", $"Количество стен 1-го этажа: {walls1.Count.ToString()} {Environment.NewLine} Количество стен 2-го этажа:{walls2.Count.ToString()}");
            return Result.Succeeded;
        }
    }
}
