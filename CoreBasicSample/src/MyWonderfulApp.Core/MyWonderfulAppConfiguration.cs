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
            var previousdir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory.TrimEnd('/', '\\')).FullName.TrimEnd('/', '\\');
            configurationBuilder
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("config.json", optional: false, reloadOnChange: true)
                .AddJsonFile(previousdir + "\\mywonderfulapp.json", optional: true)
                .AddEnvironmentVariables("MyWonderfulAppConfiguration:");

            IConfiguration configuration = configurationBuilder.Build();
            Instance = new MyWonderfulAppConfiguration();
            configuration.Bind(Instance);
        }

        public String SampleConfigurationValue { get; set; }

    }
}
