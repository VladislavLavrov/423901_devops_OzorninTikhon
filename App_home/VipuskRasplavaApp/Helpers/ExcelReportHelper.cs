using OfficeOpenXml.Style;
using OfficeOpenXml;
using static RaspredeleniyeDutyaApp.Helpers.CalculationHelper;
using RaspredeleniyeDutyaFormulas;
using RaspredeleniyeDutyaApp.Data;
using System.Reflection;

namespace RaspredeleniyeDutyaApp.Helpers
{
    public class ExcelReportHelper
    {
        ExcelPackage package = new();
        ExcelWorksheet sheet;

        public class Results
        {
            public Variant Variant { get; set; }
            public Dictionary<string, ParameterValue> PredvaritResults { get; set; }
            public Dictionary<string, ParameterValueList> DutyeResults { get; set; }
        }
        Results results;

        public ExcelReportHelper(Results results)
        {
            this.results = results;
            sheet = package.Workbook.Worksheets.Add("Отчет о расчёте распределения дутья по фурмам");
            BuildTable();
        }

        private ExcelRange Cells => sheet.Cells;

        private void BuildTable()
        {
            int row = 1;
            BuildHeader(ref row);

            BuildTableTitle(ref row, "Входные данные");
            BuildParameters(ref row, GetValueProperties(results.Variant.Data));
            row++;
            BuildFurmParameters(ref row, GetValueListProperties(results.Variant.Data));
            row++;

            BuildTableTitle(ref row, "Предварительные результаты");
            BuildParameters(ref row, results.PredvaritResults.Values);
            row++;

            BuildTableTitle(ref row, "Распределение дутья по фурмам");
            BuildFurmParameters(ref row, results.DutyeResults.Values);
            row++;

            sheet.Column(1).Width = 100;
        }

        private static IEnumerable<ParameterValue> GetValueProperties(object o)
        {
            return o.GetType()
                .GetProperties()
                .Where(p => p.PropertyType == typeof(double) || p.PropertyType == typeof(int))
                .Select(p =>
                {
                    double value = 0.0;
                    if (p.PropertyType == typeof(double)) value = (double?)p.GetValue(o) ?? 0.0;
                    else if (p.PropertyType == typeof(int)) value = (int?)p.GetValue(o) ?? 0;
                    return new ParameterValue(value, FormulasHelpers.GetDescription(p));
                });
        }

        private static ParameterValueList? ConvertPropertyList(PropertyInfo property, object o)
        {
            if (property.GetValue(o) is List<double> doubleList)
                return new ParameterValueList(doubleList, FormulasHelpers.GetDescription(property));
            else if (property.GetValue(o) is List<bool> boolList)
                return new ParameterValueList(boolList.Select(x => x ? 1.0 : 0.0).ToList(),
                    FormulasHelpers.GetDescription(property),
                    IsCheckBox: true);
            else
                return null;
        }

        private static IEnumerable<ParameterValueList?> GetValueListProperties(object o)
        {
            return o.GetType()
                .GetProperties()
                .Where(p => p.PropertyType == typeof(List<double>) || p.PropertyType == typeof(List<bool>))
                .Select(p => ConvertPropertyList(p, o));
        }

        private void BuildHeader(ref int row)
        {
            BuildTableTitle(ref row, "Отчет о расчёте распределения дутья по фурмам");
            row++;
        }

        private void BuildTableTitle(ref int row, string name)
        {
            var cell = Cells[row++, 1];
            cell.Value = name;
            SetBold(cell);
        }

        private void BuildParameters(ref int row, IEnumerable<ParameterValue> parameters)
        {
            foreach(ParameterValue param in parameters)
            {
                Cells[row, 1].Value = param.Description;
                Cells[row, 2].Value = Math.Round(param.Value, 3);
                row++;
            }
        }

        private void BuildFurmParameters(ref int row, IEnumerable<ParameterValueList?> parameters)
        {
            int nRabFurm = parameters.First()?.Value.Count ?? 0;
            Cells[row, 1].Value = "Фурма №";
            for (int i = 0; i < nRabFurm; i++)
            {
                Cells[row, i + 2].Value = i+1;
            }
            row++;

            foreach (ParameterValueList? param in parameters)
            {
                if (param is null)
                    continue;
                Cells[row, 1].Value = param.Description;
                for (int i = 0; i < nRabFurm; i++)
                {
                    var cell = Cells[row, i + 2];
                    if (results.Variant.Data.FurmPodachaDutya[i])
                    {
                        if (!param.IsCheckBox)
                            cell.Value = Math.Round(param.Value[i], 3);
                        else
                            cell.Value = param.Value[i] == 1.0 ? "Открыта" : "Закрыта";
                    }
                    else
                    {
                        cell.Value = "Закрыта";
                    }
                }
                row++;
            }
        }

        private static void SetBorder(ExcelRange range)
        {
            var style = ExcelBorderStyle.Thin;
            var border = range.Style.Border;
            border.Top.Style = style;
            border.Bottom.Style = style;
            border.Left.Style = style;
            border.Right.Style = style;
        }

        private static void SetBold(ExcelRange range)
            => range.Style.Font.Bold = true;

        public byte[] GetAsByteArray()
            => package.GetAsByteArray();

        public string GetBase64()
            => Convert.ToBase64String(GetAsByteArray());
    }
}
