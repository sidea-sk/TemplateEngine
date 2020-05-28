using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Docx.Processors.Images
{
    internal static class ImageParametersParser
    {
        private const string NumberWithUnitPattern = "(\\d+[\\.\\,]\\d*)([a-zA-Z]+)";

        private const double DPI = 72;
        private const double EMU = 914400; // == 1 Inch == 2.54cm == 127/50cm

        public static long? Width(this IEnumerable<string> parameters)
        {
            var p = parameters.FirstOrDefault(p => p.StartsWith("w"));
            return p.TryGetEmu();
        }

        public static long? Height(this IEnumerable<string> parameters)
        {
            var p = parameters.FirstOrDefault(p => p.StartsWith("h"));
            return p.TryGetEmu();
        }

        public static long PxToEmu(this int pixel)
        {
            var v = pixel * 9525;
            return v;
        }

        private static long? TryGetEmu(this string parameter)
        {
            var match = Regex.Match(parameter, NumberWithUnitPattern);
            if (!match.Success || match.Groups.Count != 3)
            {
                return null;
            }

            return TryGetEmu(match.Groups[1].Value, match.Groups[2].Value);
        }

        private static long? TryGetEmu(string number, string unit)
        {
            if(!double.TryParse(number?.Replace(',', '.'), out var n))
            {
                return null;
            }

            double v;
            switch (unit?.ToLower())
            {
                case "cm":
                    // EMU == 1inch == 2.54cm == 127/50cm, i.e. 1cm == 50/127inch = 50/127*EMU
                    v = n * 50 / 127 * EMU;
                    break;
                case "in":
                    v = n * EMU;
                    break;
                case "pt":
                    v = n * EMU / DPI;
                    break;
                case "px":
                    v = n * EMU / 9525;
                    break;
                default:
                    return null;
            }

            return (long)Math.Round(v);
        }
    }
}
