
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orderly.Shared.Helpers
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> OrderByDynamic<T>(this IEnumerable<T> source, string propertyName, bool ascending)
        {
            Func<T, object?> keySelector = x => GetNestedPropertyValue(x, propertyName);

            return ascending
                ? source.OrderBy(keySelector)
                : source.OrderByDescending(keySelector);
        }

        public static object? GetNestedPropertyValue(object obj, string propertyPath)
        {
            foreach (var prop in propertyPath.Split('.'))
            {
                if (obj == null) return null;
                var propInfo = obj.GetType().GetProperty(prop);
                if (propInfo == null) return null;
                obj = propInfo.GetValue(obj, null);
            }
            return obj;
        }
    }
}
