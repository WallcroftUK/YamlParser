using YamlParser.Core;

namespace YamlParser.Extensions
{
    public static class CreateFileExtension
    {
        public static void CreateFile(this Configuration configuration, string name, string folder, string yaml)
        {
            string portalDirectory = Path.Combine(configuration.BasePath, folder);

            if (!Directory.Exists(portalDirectory))
            {
                Directory.CreateDirectory(portalDirectory);
            }

            string filePath = Path.Combine(portalDirectory, $"{name}.yaml");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            File.WriteAllText(filePath, yaml);
        }
    }
}
