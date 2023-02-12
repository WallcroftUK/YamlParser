using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Text;
using YamlDotNet.Serialization;
using YamlParser.Core;
using YamlParser.Entities;
using YamlParser.Extensions;
using YamlParser.Shared;

namespace YamlParser.Plugins
{
    public class MapPlugin : IPlugin
    {
        private readonly Configuration _configuration;

        public MapPlugin(IOptions<Configuration> configuration)
        {
            _configuration = configuration.Value;
        }

        public void Run()
        {
            DateTime start = DateTime.Now;
            var fileMapIdLang = $"{_configuration.BasePath}_code_{_configuration.Language}_MapIDData.txt";
            var fileMapIdDat = $"{_configuration.BasePath}MapIDData.dat";

            var maps = new List<Map>();
            var dictionaryId = new Dictionary<int, string>();
            var dictionaryMusic = new Dictionary<int, int>();
            var dictionaryIdLang = new Dictionary<string, string>();

            var i = 0;
            using (var mapIdStream = new StreamReader(fileMapIdDat, Encoding.GetEncoding(1252)))
            {
                string line;
                while ((line = mapIdStream.ReadLine()) != null)
                {
                    var values = line.Split(' ');
                    if (values.Length <= 1) continue;

                    if (!int.TryParse(values[0], out var mapId)) continue;

                    if (!dictionaryId.ContainsKey(mapId)) dictionaryId.Add(mapId, values[4]);
                }
            }

            using (var mapIdLangStream = new StreamReader(fileMapIdLang, Encoding.GetEncoding(1252)))
            {
                string line;
                while ((line = mapIdLangStream.ReadLine()) != null)
                {
                    var linesave = line.Split('\t');
                    if (linesave.Length <= 1 || dictionaryIdLang.ContainsKey(linesave[0])) continue;
                    dictionaryIdLang.Add(linesave[0], linesave[1]);
                }
            }

            List<string> lines = $"{_configuration.BasePath}{_configuration.PacketName}".FilterLines(new string[] { "at " });
            foreach (string atPacket in lines)
            {
                string[] parts = atPacket.Split(" ");

                if (atPacket.Length > 7 && !dictionaryMusic.ContainsKey(int.Parse(parts[2])))
                {
                    dictionaryMusic[int.Parse(parts[2])] = int.Parse(parts[7]);
                }
            }

            foreach (var file in new DirectoryInfo($"{_configuration.BasePath}{_configuration.BinaryMapFolder}").GetFiles())
            {
                addMap(short.Parse(file.Name), short.Parse(file.Name), File.ReadAllBytes(file.FullName));
            }

            void addMap(short mapId, short originalMapId, byte[] mapData)
            {
                string name = "";
                int music = 0;

                if (dictionaryId.ContainsKey(mapId) && dictionaryIdLang.ContainsKey(dictionaryId[mapId]))
                {
                    name = dictionaryIdLang[dictionaryId[mapId]];
                }

                if (dictionaryMusic.ContainsKey(mapId))
                {
                    music = dictionaryMusic[mapId];
                }

                List<Flags> determineFlag = new();

                switch (mapId)
                {
                    case short i when i < 3 || i > 48 && i < 53 || i > 67 && i < 76 || i == 102 || i > 103 && i < 105 || i > 144 && i < 149:
                        determineFlag.Add(Flags.ACT_1);
                        break;

                    case short i when i > 19 && i < 34 || i > 52 && i < 68 || i > 84 && i < 101:
                        determineFlag.Add(Flags.ACT_2);
                        break;

                    case short i when i > 40 && i < 45 || i > 45 && i < 48 || i > 99 && i < 102 || i > 104 && i < 128 || i == 260:
                        determineFlag.Add(Flags.ACT_3);
                        break;

                    case short i when i > 129 && i <= 134 || i == 135 || i == 137 || i == 139 || i == 141 || i > 150 && i < 155 || i == 153:
                        determineFlag.Add(Flags.ACT_4);

                        // I can't fix that myself bcs I don't know map_id but someone's knowing map_id will need to fix that
                        determineFlag.Add(Flags.HAS_PVP_FACTION_ENABLED);
                        break;

                    case short i when i > 169 && i < 205:
                        determineFlag.Add(Flags.ACT_5_1);
                        break;

                    case short i when i > 204 && i < 221:
                        determineFlag.Add(Flags.ACT_5_2);
                        break;

                    case short i when i > 227 && i < 241 || i > 228 && i < 233 || i > 232 && i < 238:
                        determineFlag.Add(Flags.ACT_6_1);
                        break;

                    case short i when i > 239 && i < 251 || i == 299:
                        determineFlag.Add(Flags.ACT_6_2);
                        break;

                    case short i when i > 2627 && i < 2651:
                        determineFlag.Add(Flags.ACT_7_1);
                        break;
                }

                var map = new Map
                {
                    Music = music,
                    MapId = mapId,
                    MapVNum = mapId,
                    MapNameId = mapId,
                    GridMapId = originalMapId,
                    Data = mapData,
                    Flags = determineFlag,
                    ShopAllowed = mapId == 147
                };

                if (maps.Contains(map))
                {
                    return;
                }

                maps.Add(map);
                i++;
            }

            var serializer = new SerializerBuilder().DisableAliases().Build();
            var yaml = serializer.Serialize(maps);

            _configuration.CreateFile("official_maps.yaml", _configuration.MapFolder, yaml);

            DateTime end = DateTime.Now;
            Console.WriteLine($"Maps parsing done in {(end - start).TotalMinutes} minutes.");
        }
    }
}