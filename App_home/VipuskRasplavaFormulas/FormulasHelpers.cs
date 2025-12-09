using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RaspredeleniyeDutyaFormulas
{
    public static class FormulasHelpers
    {
        public static bool HasAttribute<T>(MemberInfo? property) where T : Attribute
            => (property?.GetCustomAttributes(typeof(T), false).Length ?? 0) > 0;

        public static T? GetAttribute<T>(MemberInfo? property) where T : Attribute
            => property?.GetCustomAttributes(typeof(T), false)[0] as T;

        public static string GetDescription(MemberInfo? property)
            => GetAttribute<DescriptionAttribute>(property)?.Description ?? "";
    }
}
