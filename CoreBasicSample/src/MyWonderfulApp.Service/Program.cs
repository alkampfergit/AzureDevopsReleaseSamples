using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Linq;

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
