using Microsoft.Extensions.Hosting;
using System.Text;
using YamlParser.Shared;

namespace YamlParser
{
    public class YamlParser : BackgroundService
    {
        private readonly IEnumerable<IPlugin> _plugins;

        public YamlParser(IEnumerable<IPlugin> plugins)
        {
            _plugins = plugins;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Yaml Parser - Dragaron");

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            foreach (var plugin in _plugins)
            {
                plugin.Run();
            }

            return Task.CompletedTask;
        }
    }
}