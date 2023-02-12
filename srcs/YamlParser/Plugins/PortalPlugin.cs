using Microsoft.Extensions.Options;
using YamlDotNet.Serialization;
using YamlParser.Core;
using YamlParser.Entities;
using YamlParser.Extensions;
using YamlParser.Shared;

namespace YamlParser.Plugins
{
    public class PortalPlugin : IPlugin
    {
        private readonly Configuration _configuration;

        public PortalPlugin(IOptions<Configuration> configuration) 
        {
            _configuration = configuration.Value;
        }

        public void Run()
        {
            DateTime start = DateTime.Now;
            Portal toSerialize = new();
            List<int> mapAlreadyDone = new();
            int map = 0;
            List<PortalAttributes> portals = new();
            List<string> filteredPortalLines = $"{_configuration.BasePath}{_configuration.PacketName}".FilterLines(new string[] { "c_map ", "gp " });

            // get each portals existing in packet.txt
            foreach (string line in filteredPortalLines)
            {
                string[] parts = line.Split(' ');

                if (parts.Length > 3 && parts[0] == "c_map")
                {
                    map = int.Parse(parts[2]);
                    continue;
                }

                if (parts.Length <= 4 || parts[0] != "gp") continue;

                if (!File.Exists($"{_configuration.BasePath}{_configuration.BinaryMapFolder}/{map}") || !File.Exists($"{_configuration.BasePath}{_configuration.BinaryMapFolder}/{int.Parse(parts[3])}")) continue;

                portals.Add(new PortalAttributes
                {
                    SourceMapId = map,
                    SourceMapX = short.Parse(parts[1]),
                    SourceMapY = short.Parse(parts[2]),
                    DestinationMapId = int.Parse(parts[3]),
                    DestinationMapX = 0,
                    DestinationMapY = 0,
                    Type = short.Parse(parts[4])
                });
            }

            var temp = portals.Distinct();
            portals = temp.ToList();

            // foreach in all portals
            foreach (PortalAttributes portal in portals)
            {
                if (mapAlreadyDone.Contains(portal.SourceMapId)) continue;

                toSerialize = new();

                // then get all portals of the same map
                foreach (PortalAttributes specialPortal in portals.Where(s => s.SourceMapId == portal.SourceMapId))
                {
                    PortalAttributes portal1 = portals.FirstOrDefault(s => s.SourceMapId == specialPortal.DestinationMapId);

                    if (portal1 == null) continue;

                    specialPortal.DestinationMapX = portal1.SourceMapX;
                    specialPortal.DestinationMapY = portal1.SourceMapY;

                    if (toSerialize.Portals != null)
                    {
                        toSerialize.Portals.Add(specialPortal);
                    }
                    else
                    {
                        toSerialize.Portals = new() { specialPortal };
                    }
                }

                var serializer = new SerializerBuilder().DisableAliases().Build();
                var yaml = serializer.Serialize(toSerialize);

                _configuration.CreateFile($"portals_{portal.SourceMapId}", _configuration.PortalFolder, yaml);
                mapAlreadyDone.Add(portal.SourceMapId);
            }

            DateTime end = DateTime.Now;
            Console.WriteLine($"Portals parsing done in {(end-start).TotalMinutes} minutes.");
        }
    }
}