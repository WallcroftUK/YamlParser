using Microsoft.Extensions.Options;
using YamlDotNet.Serialization;
using YamlParser.Core;
using YamlParser.Entities;
using YamlParser.Extensions;
using YamlParser.Shared;

namespace YamlParser.Plugins
{
    public class MapMonsterPlugin : IPlugin
    {
        private readonly Configuration _configuration;

        public MapMonsterPlugin(IOptions<Configuration> configuration)
        {
            _configuration = configuration.Value;
        }

        public void Run()
        {
            List<string> filteredMobsLines = $"{_configuration.BasePath}{_configuration.PacketName}".FilterLines(new string[] { "in", "c_map" });
            short map = 0;
            List<MapMonster> mapsMonsters = new();
            List<int> mapAlreadyDone = new();
            MapMonster toSerialize = new();

            foreach (string line in filteredMobsLines)
            {
                string[] parts = line.Split(' ');

                if (parts.Length > 3 && parts[0] == "c_map")
                {
                    map = short.Parse(parts[2]);
                    continue;
                }

                if (!File.Exists($"{_configuration.BasePath}{_configuration.BinaryMapFolder}/{map}") || !File.Exists($"{_configuration.BasePath}{_configuration.BinaryMapFolder}/{int.Parse(parts[3])}")) continue;

                if (parts.Length <= 7 || parts[0] != "in" || parts[1] != "3") continue;

                var mapMonster = mapsMonsters.FirstOrDefault(s => s.MapId == map);

                if (mapMonster == null)
                {
                    mapMonster = new MapMonster
                    {
                        MapId = map,
                        Monsters = new()
                    };
                }

                mapMonster.Monsters.Add(new()
                {
                    MapMonsterId = int.Parse(parts[3]),
                    VNum = int.Parse(parts[2]),
                    MapX = short.Parse(parts[4]),
                    MapY = short.Parse(parts[5]),
                    Position = (byte)(parts[6] == string.Empty ? 0 : byte.Parse(parts[6]))
                });

                mapsMonsters.Add(mapMonster);
            }

            var temp = mapsMonsters.Distinct();
            mapsMonsters = temp.ToList();

            foreach (MapMonster mapMonster in mapsMonsters)
            {
                if (mapAlreadyDone.Contains(mapMonster.MapId)) continue;

                toSerialize = new()
                {
                    MapId = mapMonster.MapId
                };

                foreach (MapMonster mMonster in mapsMonsters.Where(s => s.MapId == mapMonster.MapId && s.Monsters != null))
                {
                    if (toSerialize.Monsters != null)
                    {
                        toSerialize.Monsters.AddRange(mMonster.Monsters);
                    }
                    else
                    {
                        toSerialize.Monsters = mMonster.Monsters;
                    }
                }

                var serializer = new SerializerBuilder().DisableAliases().Build();
                var yaml = serializer.Serialize(toSerialize);

                _configuration.CreateFile($"map_{mapMonster.MapId}_monsters", _configuration.MapMonsterFolder, yaml);
                mapAlreadyDone.Add(mapMonster.MapId);
            }
        }
    }
}