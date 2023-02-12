using YamlDotNet.Serialization;

namespace YamlParser.Entities
{
    public record Portal
    {
        [YamlMember(Alias = "portals")]
        public List<PortalAttributes> Portals { get; set; }
    }

    public record PortalAttributes
    {
        [YamlMember(Alias = "destination_map_id")]
        public int DestinationMapId { get; set; }

        [YamlMember(Alias = "destination_map_x")]
        public short DestinationMapX { get; set; }

        [YamlMember(Alias = "destination_map_y")]
        public short DestinationMapY { get; set; }

        [YamlMember(Alias = "source_map_id")]
        public int SourceMapId { get; set; }

        [YamlMember(Alias = "source_map_x")]
        public short SourceMapX { get; set; }

        [YamlMember(Alias = "source_map_y")]
        public short SourceMapY { get; set; }

        [YamlMember(Alias = "type")]
        public short Type { get; set; }
    }
}
