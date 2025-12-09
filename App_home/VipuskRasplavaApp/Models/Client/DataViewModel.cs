namespace RaspredeleniyeDutyaApp.Models.Client
{
    public class DataViewModel
    {
        public class Parameter(object value, string description, string name)
        {
            public object Value { get; set; } = value;
            public string Description { get; set; } = description;
            public string Name { get; set; } = name;
        }
        public record ParameterValue(double Value, string Description)
        {
            public Parameter ToParameter() => new(Value, Description, "");
        }
        public record ParameterValueList(List<double> Value, string Description, bool IsCheckBox)
        {
            public Parameter ToParameter()
            {
                if (IsCheckBox)
                    return new(Value.Select(x => x == 1.0).Cast<object>().ToArray(), Description, "");
                else
                    return new(Value.Cast<object>().ToArray(), Description, "");
            }
        }

        public class Results
        {
            public Dictionary<string, ParameterValue> Predvarit { get; set; }
            public Dictionary<string, ParameterValueList> Dutye { get; set; }
        }

        public Dictionary<string, Dictionary<string, Parameter>?> InitialData { get; set; } = new();
        public Results ResultsData { get; set; }
    }
}
