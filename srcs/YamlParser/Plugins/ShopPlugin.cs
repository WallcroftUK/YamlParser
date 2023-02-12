//using Microsoft.Extensions.Options;
//using YamlDotNet.Serialization;
//using YamlParser.Core;
//using YamlParser.Entities;
//using YamlParser.Extensions;
//using YamlParser.Shared;

//namespace YamlParser.Plugins
//{
//    public class ShopPlugin : IPlugin
//    {
//        private readonly Configuration _configuration;

//        public ShopPlugin(IOptions<Configuration> configuration)
//        {
//            _configuration = configuration.Value;
//        }

//        public void Run()
//        {
//            //List<Shop> shops = new();
//            //List<string> filteredShopLines = $"{_configuration.BasePath}{_configuration.PacketName}".FilterLines(new string[] { "shop " });

//            //foreach (string line in filteredShopLines)
//            //{
//            //    var temp = line.Split(" ");
//            //    var name = line[line.IndexOf(temp[5 + 1])..];
//            //    shops.Add(new Shop
//            //    {
//            //        ShopType = temp[0],
//            //        ShopId = temp[1],
//            //        MenuType = temp[2],
//            //        Unknown = temp[3],
//            //        Unknown2 = temp[4],
//            //        Type = temp[5],
//            //        Name = name
//            //    });
//            //}

//            //var serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
//            //var yaml = serializer.Serialize(shops);

//            DateTime start = DateTime.Now;
//            Shop toSerialize = new();
//            List<int> mapAlreadyDone = new();
//            List<ShopAttributes> shops = new();
//            List<string> filteredShopLines = $"{_configuration.BasePath}{_configuration.PacketName}".FilterLines(new string[] { "shop " });

//            // get each portals existing in packet.txt
//            foreach (string line in filteredShopLines)
//            {
//                string[] parts = line.Split(' ');

//                var name = line[line.IndexOf(parts[5 + 1])..];

//                shops.Add(new ShopAttributes
//                {
//                    ShopType = parts[0],
//                    ShopId = parts[1],
//                    MenuType = parts[2],
//                    Unknown = parts[3],
//                    Unknown2 = parts[4],
//                    Type = parts[5],
//                    Name = name
//                });
//            }

//            var temp = shops.Distinct();
//            shops = temp.ToList();

//            // foreach in all portals
//            foreach (ShopAttributes portal in shops)
//            {
//                if (mapAlreadyDone.Contains(portal.Map)) continue;

//                toSerialize = new();

//                // then get all portals of the same map
//                foreach (PortalAttributes specialPortal in portals.Where(s => s.SourceMapId == portal.SourceMapId))
//                {
//                    PortalAttributes portal1 = portals.FirstOrDefault(s => s.SourceMapId == specialPortal.DestinationMapId);

//                    if (portal1 == null) continue;

//                    specialPortal.DestinationMapX = portal1.SourceMapX;
//                    specialPortal.DestinationMapY = portal1.SourceMapY;

//                    if (toSerialize.Portals != null)
//                    {
//                        toSerialize.Portals.Add(specialPortal);
//                    }
//                    else
//                    {
//                        toSerialize.Portals = new() { specialPortal };
//                    }
//                }

//                var serializer = new SerializerBuilder().Build();
//                var yaml = serializer.Serialize(toSerialize);
//                string portalDirectory = $"{_configuration.BasePath}{_configuration.PortalFolder}";
//                if (!Directory.Exists(portalDirectory))
//                {
//                    Directory.CreateDirectory(portalDirectory);
//                }

//                string filePattern = $"{portalDirectory}/portals_{portal.SourceMapId}.yaml";
//                if (File.Exists(filePattern))
//                {
//                    File.Delete(filePattern);
//                }

//                File.WriteAllText($"{portalDirectory}/portals_{portal.SourceMapId}.yaml", yaml);
//                mapAlreadyDone.Add(portal.SourceMapId);
//            }

//            DateTime end = DateTime.Now;
//            Console.WriteLine($"Shop parsing done in {(end - start).TotalMinutes} minutes.");
//        }
//    }
//}