using System;
using System.Collections.Generic;

namespace Dwapi.Bot.SharedKernel.Utility
{
    public static class Extensions
    {
        public static IEnumerable<IEnumerable<T>> Chunks<T>(this IEnumerable<T> enumerable,
            int chunkSize)
        {
            if (chunkSize < 1) throw new ArgumentException("chunkSize must be positive");

            using (var e = enumerable.GetEnumerator())
                while (e.MoveNext())
                {
                    var remaining = chunkSize;    // elements remaining in the current chunk
                    var innerMoveNext = new Func<bool>(() => --remaining > 0 && e.MoveNext());

                    yield return e.GetChunk(innerMoveNext);
                    while (innerMoveNext()) {/* discard elements skipped by inner iterator */}
                }
        }

        private static IEnumerable<T> GetChunk<T>(this IEnumerator<T> e,
            Func<bool> innerMoveNext)
        {
            do yield return e.Current;
            while (innerMoveNext());
        }

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
