
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orderly.Shared.Helpers
{
    public class StringHelper
    {
        public static string Truncate(string input, int maxLength, bool addEllipsis = true)
        {
            if (string.IsNullOrEmpty(input) || maxLength <= 0)
            {
                return string.Empty;
            }

            if (input.Length <= maxLength)
            {
                return input;
            }

            return addEllipsis && maxLength > 3
                ? input.Substring(0, maxLength - 1) + "…"
                : input.Substring(0, maxLength);
        }
    }
}
