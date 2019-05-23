using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace MyWonderfulApp.Core
{
    public class MyWonderfulAppConfiguration
    {
        public static MyWonderfulAppConfiguration Instance { get; private set; }

        static MyWonderfulAppConfiguration()
        {
            InitConfiguration();
        }

        private static void InitConfiguration()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("config.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables("MyWonderfulAppConfiguration:");

            IConfiguration configuration = configurationBuilder.Build();
            Instance = new MyWonderfulAppConfiguration();
            configuration.Bind(Instance);
        }

        public String SampleConfigurationValue { get; set; }

        private static string GetConfigFile()
        {
            var currentDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('/', '\\');
            Console.WriteLine($"App directory is {currentDir}");
            var parentPath = Directory.GetParent(currentDir).FullName;
            Console.WriteLine($"Parent app directory is {parentPath}");
            var parentConfigfile = Path.Combine(parentPath, "mywonderfulapp.json");
            Console.WriteLine($"searching for  {parentConfigfile}");
            if (File.Exists(parentConfigfile))
            {
                Console.WriteLine($"Config file found:  {parentConfigfile}");
                return parentConfigfile;
            }
            Console.WriteLine($"Config file not found, resort to standard config.json");
            return Path.Combine(currentDir, "config.json");
        }
    }
}
