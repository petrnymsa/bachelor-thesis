using System;
using System.Collections.Generic;
using System.Text;

namespace BachelorThesis.Business
{
    public static class IntExtension
    {
        public static int? ToNullableInt(this string s)
        {
            return int.TryParse(s, out var i) ? i : (int?)null;
        }
    }
}
