namespace YamlParser.Extensions
{
    public static class StringExtensions
    {
        public static List<string> FilterLines(this string path, string[] startWith)
        {
            return File.ReadLines(path)
                .Where(line => startWith.Any(str => line.StartsWith(str)))
                .ToList();
        }
    }
}
