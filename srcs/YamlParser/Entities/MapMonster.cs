using YamlDotNet.Serialization;

namespace YamlParser.Entities
{
    public record MapMonster
    {
        [YamlMember(Alias = "map_id")]
        public int MapId { get; set; }

        [YamlMember(Alias = "monsters")]
        public List<Monster> Monsters { get; set; }
    }

    public record Monster
    {
        [YamlMember(Alias = "map_monster_id")]
        public int MapMonsterId { get; set; }

        [YamlMember(Alias = "vnum")]
        public int VNum { get; set; }

        [YamlMember(Alias = "map_x")]
        public int MapX { get; set; }

        [YamlMember(Alias = "map_y")]
        public int MapY { get; set; }

        [YamlMember(Alias = "position")]
        public int Position { get; set; }

        [YamlMember(Alias = "can_move")]
        public bool CanMove { get; set; }
    }
}