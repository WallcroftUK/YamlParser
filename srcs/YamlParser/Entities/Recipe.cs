using YamlDotNet.Serialization;

namespace YamlParser.Entities
{
    public record Recipe
    {
        [YamlMember(Alias = "recipes")]
        public List<RecipeAttributes> Recipes { get; set; }
    }

    public record RecipeAttributes
    {
        [YamlMember(Alias = "item_vnum")]
        public long ItemVnum { get; set; }

        [YamlMember(Alias = "quantity")]
        public long Amount { get; set; }

        [YamlMember(Alias = "producer_item_vnum")]
        public long ProducerItemVnum { get; set; }

        [YamlMember(Alias = "items")]
        public List<ItemAttributes> Items { get; set; }
    }

    public record ItemAttributes
    {
        [YamlMember(Alias = "item_vnum")]
        public long ItemVNum { get; set; }

        [YamlMember(Alias = "quantity")]
        public long Amount { get; set; }
    }
}
