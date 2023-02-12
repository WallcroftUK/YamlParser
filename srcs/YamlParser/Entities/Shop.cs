namespace YamlParser.Entities
{
    public record Shop
    {
        public List<ShopAttributes> Shops { get; set; }
    }

    public record ShopAttributes
    {
        public string ShopType { get; set; }

        public string ShopId { get; set; }

        public string MenuType { get; set; }

        public string Unknown { get; set; }

        public string Unknown2 { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }
    }
}
