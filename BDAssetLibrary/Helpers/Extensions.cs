using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDAssetLibrary.Helpers
{
    public static class Extensions
    {
        public static string Capitalize(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;
            if (str[0] >= 97 && str[0] <= 122)
                return (char)(str[0] - 32) + str.Substring(1);

            return str;
        }
    }
}
