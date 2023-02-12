using YamlParser.Core;

namespace YamlParser.Extensions
{
    public static class CreateFileExtension
    {
        public static void CreateFile(this Configuration configuration, string name, string folder, string yaml)
        {
            string portalDirectory = $"{configuration.BasePath}{folder}";
            if (!Directory.Exists(portalDirectory))
            {
                Directory.CreateDirectory(portalDirectory);
            }

            string filePattern = $"{portalDirectory}/{name}.yaml";
            if (File.Exists(filePattern))
            {
                File.Delete(filePattern);
            }

            File.WriteAllText($"{portalDirectory}/{name}.yaml", yaml);
        }
    }
}