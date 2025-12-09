using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspredeleniyeDutyaFormulas
{
    [AttributeUsage(AttributeTargets.Method)]
    public class FormulaAttribute(string description, bool allows_negative = false) : DescriptionAttribute(description)
    {
        public bool AllowsNegative { get; set; } = allows_negative;
    }
}
