using System;

namespace Dwapi.Bot.Core.Utility
{
    public static class Extensions
    {
        public static string HasToEndWith(this string value, string end)
        {
            if (value == null)
                return string.Empty;

            return value.EndsWith(end) ? value : $"{value}{end}";
        }

        public static string ToOsStyle(this string value)
        {
            if (value == null)
                return string.Empty;

            if(Environment.OSVersion.Platform==PlatformID.Unix||Environment.OSVersion.Platform==PlatformID.MacOSX)
                return value.Replace(@"\", @"/");

            return value.Replace(@"/", @"\");
        }
    }
}
