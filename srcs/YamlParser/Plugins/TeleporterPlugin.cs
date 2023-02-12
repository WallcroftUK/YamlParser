//using Microsoft.Extensions.Options;
//using YamlDotNet.Serialization;
//using YamlDotNet.Serialization.NamingConventions;
//using YamlParser.Core;
//using YamlParser.Entities;
//using YamlParser.Extensions;
//using YamlParser.Shared;

//namespace YamlParser.Plugins
//{
//    public class TeleporterPlugin : IPlugin
//    {
//        private readonly Configuration _configuration;

//        public TeleporterPlugin(IOptions<Configuration> configuration)
//        {
//            _configuration = configuration.Value;
//        }

//        public void Run()
//        {
//            List<Teleporter> teleporters = new();
//            List<string> filteredTeleporterLines = _configuration.PacketPath.FilterLines(new string[] { "at ", "n_run " });
//            Teleporter teleporter = new();

//            foreach (string line in filteredTeleporterLines)
//            {
//                string[] parts = line.Split(" ");

//                if (parts[1] != "16" && parts[1] != "26" && parts[1] != "45" && parts[1] != "301" &&
//                    parts[1] != "132" && parts[1] != "5002" && parts[1] != "5012")
//                {
//                    continue;
//                }

//                if (parts.Length > 4 && parts[0] == "n_run")
//                {
//                    teleporter.Teleporters.Add(new Teleporters()
//                    {
//                        MapNpcId = int.Parse(parts[4]),
//                        Type = 1
//                    });

//                    continue;
//                }

//                if (parts.Length <= 5 || parts[0] != "at" || teleporter == null)
//                {
//                    continue;
//                }

//                teleporter.MapId = short.Parse(parts[2]);
//                var lastTp = teleporter.Teleporters.Last();
//                lastTp.MapX = short.Parse(parts[3]);
//                lastTp.MapY = short.Parse(parts[4]);

//                teleporters.Add(teleporter);
//            }

//            var serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
//            var yaml = serializer.Serialize(teleporters);
//            Console.WriteLine(yaml);
//        }
//    }
//}
