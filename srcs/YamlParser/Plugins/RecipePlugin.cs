using Microsoft.Extensions.Options;
using YamlDotNet.Serialization;
using YamlParser.Core;
using YamlParser.Entities;
using YamlParser.Extensions;
using YamlParser.Shared;

namespace YamlParser.Plugins
{
    public class RecipePlugin : IPlugin
    {
        private readonly Configuration _configuration;

        public RecipePlugin(IOptions<Configuration> configuration)
        {
            _configuration = configuration.Value;
        }

        public void Run()
        {
            DateTime start = DateTime.Now;
            string[] lines = File.ReadAllLines(Path.Combine(_configuration.BasePath, _configuration.RecipeFile));
            List<Recipe> recipes = new();
            Recipe tempRecipe = new();
            long vnumToInsert = 0;

            foreach (string line in lines)
            {
                string[] parts = line.Split(' ');

                if (line.StartsWith("m_list 2"))
                {
                    foreach (string part in parts.Skip(3))
                    {
                        if (tempRecipe.Recipes == null)
                        {
                            tempRecipe.Recipes = new List<RecipeAttributes>()
                            {
                                new RecipeAttributes
                                {
                                    ItemVnum = Convert.ToInt64(part),
                                    ProducerItemVnum = Convert.ToInt64(parts[2]),
                                    Amount = 1
                                }
                            };
                        }
                        else
                        {
                            tempRecipe.Recipes.Add(new RecipeAttributes
                            {
                                ItemVnum = Convert.ToInt64(part),
                                ProducerItemVnum = Convert.ToInt64(parts[2]),
                                Amount = 1
                            });
                        }
                    }
                }
                
                if (line.StartsWith("pdtse"))
                {
                    vnumToInsert = Convert.ToInt64(parts[2]);
                }

                if (line.StartsWith("m_list 3"))
                {
                    for (int i = 3; i < parts.Length; i += 2)
                    {
                        if (Convert.ToInt64(parts[i]) < 0) continue;

                        if (tempRecipe.Recipes == null) continue;

                        var findRecipeRelatedToVnum = tempRecipe.Recipes.FirstOrDefault(s => s.ItemVnum == vnumToInsert);

                        if (findRecipeRelatedToVnum == null) break;

                        if (findRecipeRelatedToVnum.Items == null)
                        {
                            findRecipeRelatedToVnum.Items = new List<ItemAttributes>()
                            {
                                new ItemAttributes()
                                {
                                    ItemVNum = Convert.ToInt64(parts[i]),
                                    Amount = Convert.ToInt64(parts[i + 1])
                                }
                            };
                        }
                        else
                        {
                            if (findRecipeRelatedToVnum.Items.FirstOrDefault(s => s.ItemVNum == Convert.ToInt64(parts[i])) != null) continue;

                            findRecipeRelatedToVnum.Items.Add(new ItemAttributes()
                            {
                                ItemVNum = Convert.ToInt64(parts[i]),
                                Amount = Convert.ToInt64(parts[i + 1])
                            });
                        }
                    }
                }

                if (line.StartsWith("m_list 6"))
                {
                    for (int i = 2; i < parts.Length; i += 2)
                    {
                        if (Convert.ToInt64(parts[i]) < 0) continue;

                        if (tempRecipe.Recipes == null) continue;

                        var findRecipeRelatedToVnum = tempRecipe.Recipes.FirstOrDefault(s => s.ItemVnum == vnumToInsert);

                        if (findRecipeRelatedToVnum == null) break;

                        if (findRecipeRelatedToVnum.Items == null)
                        {
                            findRecipeRelatedToVnum.Items = new List<ItemAttributes>()
                            {
                                new ItemAttributes()
                                {
                                    ItemVNum = Convert.ToInt64(parts[i]),
                                    Amount = Convert.ToInt64(parts[i + 1])
                                }
                            };
                        }
                        else
                        {
                            if (findRecipeRelatedToVnum.Items.FirstOrDefault(s => s.ItemVNum == Convert.ToInt64(parts[i])) != null) continue;

                            findRecipeRelatedToVnum.Items.Add(new ItemAttributes()
                            {
                                ItemVNum = Convert.ToInt64(parts[i]),
                                Amount = Convert.ToInt64(parts[i + 1])
                            });
                        }
                    }
                }

                if (line.StartsWith("pdtclose"))
                {
                    recipes.Add(tempRecipe);
                    tempRecipe = new();
                }
            }


            Recipe toSerialize = new();
            List<long> alreadyDone = new();
            foreach (Recipe recipe in recipes)
            {
                toSerialize = new();

                if (recipe.Recipes == null) continue;

                if (alreadyDone.Contains(recipe.Recipes[0].ProducerItemVnum)) continue;

                foreach (Recipe r in recipes.Where(s => s.Recipes?.First().ProducerItemVnum == recipe.Recipes?.First().ProducerItemVnum))
                {
                    toSerialize = r;
                }

                alreadyDone.Add(recipe.Recipes[0].ProducerItemVnum);
                var serializer = new SerializerBuilder().DisableAliases().Build();
                var yaml = serializer.Serialize(toSerialize);
                _configuration.CreateFile($"item_{recipe.Recipes[0].ProducerItemVnum}_recipes", _configuration.RecipeFolder, yaml);
                Console.WriteLine(yaml);
            }

            DateTime end = DateTime.Now;
            Console.WriteLine($"Recipe parsing done in {(end-start).TotalMinutes} minutes.");
        }
    }
}