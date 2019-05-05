using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MyWonderfulApp.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseContentRoot(FindAppRoot())
                .UseWebRoot("www")
                .UseIISIntegration()
                .UseStartup<Startup>();

        private static string FindAppRoot()
        {
            var root = AppDomain.CurrentDomain.BaseDirectory
                .ToLowerInvariant()
                .Split(System.IO.Path.DirectorySeparatorChar)
                .ToList();

            while (true)
            {
                var last = root.Last();
                if (last?.Length == 0
                    || last == "debug"
                    || last == "release"
                    || last == "bin"
                    || last == "netcoreapp2.2")
                {
                    root.RemoveAt(root.Count - 1);
                    continue;
                }

                break;
            }

            return String.Join("" + System.IO.Path.DirectorySeparatorChar, root);
        }
    }
}
