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
        private readonly List<int> _mapAlreadyDone;
        private readonly Serializer _serializer;

        public MapMonsterPlugin(IOptions<Configuration> configuration)
        {
            _configuration = configuration.Value;
            _serializer = new SerializerBuilder().DisableAliases().Build();
            _mapAlreadyDone = new();
        }

        public void Run()
        {
            var filteredMobsLines = Path.Combine(_configuration.BasePath, _configuration.PacketName).FilterLines(new[] { "in", "c_map" });
            var mapsMonsters = new List<MapMonster>();

            foreach (var line in filteredMobsLines)
            {
                var parts = line.Split(' ');
                if (parts.Length <= 3 || parts[0] != "c_map") continue;

                var map = short.Parse(parts[2]);
                if (!File.Exists(Path.Combine(_configuration.BasePath, _configuration.BinaryMapFolder, map.ToString())) ||
                    !File.Exists(Path.Combine(_configuration.BasePath, _configuration.BinaryMapFolder, parts[3])))
                {
                    continue;
                }

                if (parts.Length <= 7 || parts[0] != "in" || parts[1] != "3") continue;

                var mapMonster = mapsMonsters.FirstOrDefault(s => s.MapId == map);
                if (mapMonster == null)
                {
                    mapMonster = new MapMonster
                    {
                        MapId = map,
                        Monsters = new List<MapMonsterData>()
                    };
                }

                mapMonster.Monsters.Add(new MapMonsterData
                {
                    MapMonsterId = int.Parse(parts[3]),
                    VNum = int.Parse(parts[2]),
                    MapX = short.Parse(parts[4]),
                    MapY = short.Parse(parts[5]),
                    Position = (byte)(parts[6] == string.Empty ? 0 : byte.Parse(parts[6]))
                });

                mapsMonsters.Add(mapMonster);
            }

            mapsMonsters = mapsMonsters.Distinct().ToList();

            foreach (var mapMonster in mapsMonsters)
            {
                if (_mapAlreadyDone.Contains(mapMonster.MapId)) continue;

                var toSerialize = new MapMonster
                {
                    MapId = mapMonster.MapId,
                    Monsters = mapsMonsters
                        .Where(s => s.MapId == mapMonster.MapId && s.Monsters != null)
                        .SelectMany(mMonster => mMonster.Monsters)
                        .ToList()
                };

                var yaml = _serializer.Serialize(toSerialize);
                _configuration.CreateFile($"map_{mapMonster.MapId}_monsters", _configuration.MapMonsterFolder, yaml);
                _mapAlreadyDone.Add(mapMonster.MapId);
            }
        }
    }
}
