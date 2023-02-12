namespace YamlParser.Entities
{
    public record Teleporter
    {
        public short MapId { get; set; }

        public List<Teleporters> Teleporters { get; set; }
    }

    public record Teleporters
    {
        public int MapNpcId { get; set; }

        public short MapX { get; set; }

        public short MapY { get; set; }

        public byte Type { get; set; }
    }
}
