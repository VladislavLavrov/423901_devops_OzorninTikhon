using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspredeleniyeDutyaFormulas
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class DescriptionAttribute(string description) : Attribute
    {
        public string Description { get; set; } = description;
    }
}
