using YamlDotNet.Serialization;

namespace YamlParser.Entities
{
    public record Map
    {
        [YamlIgnore]
        public byte[] Data { get; set; }

        [YamlIgnore]
        public short GridMapId { get; set; }

        [YamlMember(Alias = "map_name_id")]
        public short MapNameId { get; set; }

        [YamlMember(Alias = "map_id")]
        public short MapId { get; set; }

        [YamlMember(Alias = "map_vnum")]
        public short MapVNum { get; set; }

        [YamlMember(Alias = "map_music_id")]
        public int Music { get; set; }

        [YamlMember(Alias = "flags")]
        public List<Flags> Flags { get; set; }

        [YamlIgnore]
        public bool ShopAllowed { get; set; }
    }

    public enum Flags
    {
        ACT_1 = 0,
        ACT_2 = 1,
        ACT_3 = 2,
        ACT_4 = 3,
        ACT_5_1 = 4,
        ACT_5_2 = 5,
        ACT_6_1 = 6,
        ACT_6_2 = 7,
        ACT_7_1 = 8,
        ACT_7_2 = 9,
        ACT_8 = 10,

        ANGEL_SIDE = 30,
        DEMON_SIDE = 31,

        NOSVILLE = 40,
        PORT_ALVEUS = 41,

        IS_BASE_MAP = 50,
        IS_MINILAND_MAP = 51,

        HAS_PVP_ENABLED = 100,
        HAS_PVP_FACTION_ENABLED = 101,
        HAS_PVP_FAMILY_ENABLED = 102,
        HAS_USER_SHOPS_DISABLED = 103,
        HAS_DROP_DIRECTLY_IN_INVENTORY_ENABLED = 104,
        HAS_CHAMPION_EXPERIENCE_ENABLED = 106,
        HAS_SEALED_VESSELS_DISABLED = 107,
        HAS_RAID_TEAM_SUMMON_STONE_ENABLED = 108,
        HAS_SIGNPOSTS_ENABLED = 110,

        HAS_IMMUNITY_ON_MAP_CHANGE_ENABLED = 200,
        HAS_BURNING_SWORD_ENABLED = 201
    }
}
