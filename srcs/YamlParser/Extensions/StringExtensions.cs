namespace YamlParser.Extensions
{
    public static class StringExtensions
    {
        public static List<string> FilterLines(this string path, string[] startWith)
        {
            return File.ReadLines(path)
                .Where(line =>
                {
                    foreach (var str in startWith)
                    {
                        if (line.StartsWith(str))
                        {
                            return true;
                        }
                    }
                    return false;
                })
                .ToList();
        }
    }
}