using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using Utility.Extensions.Configuration.Yaml;
using YamlParser.Core;
using YamlParser.Plugins;
using YamlParser.Shared;

namespace YamlParser
{
    public static class YamlParserBootstrap
    {
        public static async Task Main(string[] args)
        {
            try
            {
                await BuildHost(args).RunAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static IHost BuildHost(string[] _)
        {
            return new HostBuilder()
                .UseConsoleLifetime()
                .ConfigureAppConfiguration((_, conf) =>
                {
                    conf.AddYamlFile($@"{Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent}\config.yml", false, true)
                        .AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    Console.Title = "Yaml Parse - Dragaron";

                    var configuration = new Configuration();
                    hostContext.Configuration.Bind(configuration);
                    services.AddOptions<Configuration>().Bind(hostContext.Configuration);

                    var type = typeof(IPlugin);
                    var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(s => type.IsAssignableFrom(s)).Where(s => !s.IsInterface);

                    foreach (Type plugin in types)
                    {
                        services.AddSingleton(typeof(IPlugin), plugin);
                    }

                    services.AddHostedService<YamlParser>();
                })
                .Build();
        }
    }
}
