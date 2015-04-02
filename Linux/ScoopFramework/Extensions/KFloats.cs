using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoopFramework.Extensions
{
    public static class KFloats
    {
        public static string ToKFloats(this float value)
        {
            if (value > 999999999999999999) return value.ToString("0") + "E";
            if (value > 999999999999999) return value.ToString("0") + "P";
            if (value > 999999999999) return value.ToString("0") + "T";
            if (value > 999999999) return value.ToString("0") + "G";
            if (value > 999999) return value.ToString("0") + "M";
            if (value > 999) return value.ToString("0") + "K";
            if (value >= 0) return value.ToString("0");
            if (value > 0.001f) return (value*1000).ToString("0")+"m";
            if (value > 0.000001f) return (value * 1000000).ToString("0") + "µ";
            if (value > 0.000000001f) return (value * 1000000000).ToString("0") + "n";
            if (value > 0.000000000001f) return (value * 1000000000000).ToString("0") + "p";
            if (value > 0.000000000000001f) return (value * 1000000000000000).ToString("0") + "f";
            return value.ToString();
        }
    }
}
