using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using OfficeOpenXml.FormulaParsing.Excel.Functions;
using RaspredeleniyeDutyaFormulas;

namespace RaspredeleniyeDutyaApp.Helpers
{
    public class CalculationHelper
    {
        public record Parameter(string Name, string Description, string Type, object DefaultValue);
        public record ParameterValue(double Value, string Description);
        public record ParameterValueList(List<double> Value, string Description, bool IsCheckBox = false);

        Controller _controller;

        public CalculationHelper(Controller controller)
        {
            _controller = controller;
        }

        private static string ToCamelCase(string s)
            => s[0].ToString().ToLower() + s[1..];

        private static string GetTypeName(Type type)
        {
            if (type == typeof(List<double>))
                return "List<double>";
            if (type == typeof(List<bool>))
                return "List<bool>";
            return type.Name;
        }

        private JsonResult GetParameters<T>(T? obj) where T: class, new()
        {
            obj ??= new();
            return _controller.Json(typeof(T).GetProperties().Select(
                x => new Parameter(
                    ToCamelCase(x.Name),
                    FormulasHelpers.GetDescription(x),
                    GetTypeName(x.PropertyType),
                    x.GetValue(obj) ?? 0)
                ).ToList());
        }

        public JsonResult Parameters(InitialData? data)
            => GetParameters(data);

        public static bool IsInitialDataValid(InitialData data)
        {
            foreach(var property in typeof(InitialData).GetProperties())
            {
                if (property.PropertyType == typeof(double))
                {
                    if ((double?)property.GetValue(data) < 0)
                        return false;
                }
                else if (property.PropertyType == typeof(int))
                {
                    if ((int?)property.GetValue(data) <= 0)
                        return false;
                }
                else if (property.PropertyType == typeof(List<double>))
                {
                    List<double> list = (List<double>)property.GetValue(data)!;
                    foreach(double value in list)
                    {
                        if (value < 0)
                            return false;
                    }
                }
                // Тут мы игнорируем List<bool>, потому что в bool есть только 2 значения,
                // и неправильного значения быть не может.
            }
            return true;
        }

        public Dictionary<string, ParameterValue>? Calculate<TData, TFormulas>(TData data, params object[] additionalData)
        {
            Dictionary<string, ParameterValue> result = [];

            if (data is null)
                return result;

            foreach (var formula in typeof(TFormulas).GetMethods())
            {
                if (!FormulasHelpers.HasAttribute<FormulaAttribute>(formula))
                    continue;
                
                double value = (double?)formula.Invoke(null, additionalData.ToList().Prepend(data).ToArray()) ?? 0.0;
                if (double.IsInfinity(value) || double.IsNaN(value))
                    throw new ArgumentException("Значение бесконечности или NaN в результатах расчёта.");

                FormulaAttribute attr = FormulasHelpers.GetAttribute<FormulaAttribute>(formula)!;

                // Были введены неверные входные данные, из-за чего получился отрицательный результат в формуле.
                if (value < 0 && !attr.AllowsNegative)
                    return null;

                result[ToCamelCase(formula.Name)] = new ParameterValue(value, FormulasHelpers.GetDescription(formula));
            }

            return result;
        }

        public Dictionary<string, ParameterValue>? CalculatePredvarit(InitialData data)
            => Calculate<InitialData, PredvaritFormulas>(data);

        public Dictionary<string, ParameterValueList>? CalculateDutye(InitialData data)
        {
            Dictionary<string, ParameterValueList> result = [];

            if (data.NRabFurm <= 0)
                return result;

            var firstResult = Calculate<InitialData, DutyeFormulas>(data, 0);
            if (firstResult == null)
                return null;

            foreach (KeyValuePair<string, ParameterValue> item in firstResult)
            {
                double value = data.FurmPodachaDutya[0] ? item.Value.Value : 0.0;
                result[item.Key] = new ParameterValueList([value], item.Value.Description);
            }

            for (int i = 1; i < data.NRabFurm; i++)
            {
                if (data.FurmPodachaDutya[i])
                {
                    var furmResult = Calculate<InitialData, DutyeFormulas>(data, i);
                    if (furmResult == null)
                        return null;
                    foreach (KeyValuePair<string, ParameterValue> item in furmResult)
                        result[item.Key].Value.Add(item.Value.Value);
                }
                else
                {
                    foreach (KeyValuePair<string, ParameterValue> item in firstResult)
                        result[item.Key].Value.Add(0.0);
                }
            }

            return result;
        }

        public Dictionary<string, Dictionary<string, object>>? CalculateVariant(InitialData data)
        {
            if (!IsInitialDataValid(data))
                return null;

            Dictionary<string, Dictionary<string, object>> result = [];
            Dictionary<string, object>? predvarit = CalculatePredvarit(data)?.ToDictionary(x => x.Key, x => (object)x.Value);
            Dictionary<string, object>? dutye = CalculateDutye(data)?.ToDictionary(x => x.Key, x => (object)x.Value);

            // Результаты, как и входные данные, неверные
            if (predvarit == null || dutye == null)
                return null;

            result["predvarit"] = predvarit;
            result["dutye"] = dutye;
            return result;
        }
    }
}
