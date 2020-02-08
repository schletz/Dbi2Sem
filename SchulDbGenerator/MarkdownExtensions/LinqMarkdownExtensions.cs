// *************************************************************************************************
// Anpassung des Codes von jpierson/to-markdown-table
// Quelle: https://github.com/jpierson/to-markdown-table
// 1. Komplexe Typen werden nicht ausgegeben
// 2. Nullable Typen werden auch als numerisch erkannt.
// 3. Eine 3stellige Zeilennummer wird als erste Spalte (#) ausgegeben.
// *************************************************************************************************
using System.Collections.Generic;
using System.Reflection;

namespace System.Linq
{
    public static class LinqMarkdownExtensions
    {
        static int counter = 1;
        public static void Write(this string value)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine(value);
        }
        public static void WriteItem(this string value)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine($"{Environment.NewLine}**({counter++})** {value.Trim()}");
        }

        public static void WriteMarkdown<T>(this IEnumerable<T> source)
        {
            Console.WriteLine();                       // Vor der Tabelle muss eine Leerzeile sein!
            Console.WriteLine(source.ToMarkdownTable());
        }

        public static string ToMarkdownTable<T>(this IEnumerable<T> source)
        {
            var isNumeric = new Func<Type, bool>(type =>
                type == typeof(byte) || type == typeof(byte?) ||
                type == typeof(short) || type == typeof(short?) ||
                type == typeof(int) || type == typeof(int?) ||
                type == typeof(long) || type == typeof(long?) ||
                type == typeof(float) || type == typeof(float?) ||
                type == typeof(double) || type == typeof(double?) ||
                type == typeof(decimal) || type == typeof(decimal?));

            var isAllowed = new Func<Type, bool>(type =>
                isNumeric(type) ||
                type == typeof(bool) || type == typeof(bool?) ||
                type == typeof(char) || type == typeof(char?) ||
                type == typeof(string) ||
                type == typeof(DateTime) || type == typeof(DateTime?));

            var properties = typeof(T).GetRuntimeProperties().Where(p => isAllowed(p.PropertyType));
            var gettables = from p in properties
                            select new
                            {
                                p.Name,
                                GetValue = (Func<object, object>)p.GetValue,
                                Type = p.PropertyType
                            };


            var maxColumnValues = source
                .Select(x => gettables.Select(p => p.GetValue(x)?.ToString()?.Length ?? 0))
                .Union(new[] { gettables.Select(p => p.Name.Length) }) // Include header in column sizes
                .Aggregate(
                    new int[gettables.Count()].AsEnumerable(),
                    (accumulate, x) => accumulate.Zip(x, Math.Max))
                .ToArray();

            var columnNames = gettables.Select(p => p.Name);
            var headerLine = "| #   | " + string.Join(" | ", columnNames.Select((n, i) => n.PadRight(maxColumnValues[i]))) + " |";
            var rightAlign = new Func<Type, char>(type => isNumeric(type) ? ':' : ' ');

            var headerDataDividerLine =
                "| ---:| " +
                 string.Join(
                     "| ",
                     gettables.Select((g, i) => new string('-', maxColumnValues[i]) + rightAlign(g.Type))) +
                "|";

            int i = 1;
            var lines = new[]
                {
                    headerLine,
                    headerDataDividerLine,
                }.Union(
                    source
                    .Select(s =>
                        "| " + (i++).ToString().PadLeft(3) + " | " + string.Join(" | ",
                            gettables
                                .Select((n, i) => (n.GetValue(s)?.ToString() ?? "")
                                .PadRight(maxColumnValues[i]))) + " |"));

            return lines
                .Aggregate((p, c) => p + Environment.NewLine + c);
        }
    }
}
