using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MVCClient21
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {

            //var config = new ConfigurationBuilder()
            //        .SetBasePath(Directory.GetCurrentDirectory())
            //        .AddJsonFile("hosting.json", optional: false)
            //        .AddCommandLine(args)
            //        .Build();

            //var host = new WebHostBuilder()
            //    .UseKestrel()
            //    .UseConfiguration(config)
            //    .UseContentRoot(Directory.GetCurrentDirectory())
            //    .UseIISIntegration()
            //    .UseStartup<Startup>();
            //return host;

            return WebHost.CreateDefaultBuilder(args)
                //.ConfigureAppConfiguration((hostingContext, config) =>
                //{
                //    config.SetBasePath(Directory.GetCurrentDirectory());
                //    config.AddJsonFile("hosting.json", optional: false, reloadOnChange: false);
                //    config.AddCommandLine(args);
                //})
                //.UseConfiguration(config)
                //.UseUrls("http://localhost:60000", "https://localhost:60001")
                //.UseKestrel()
                .UseStartup<Startup>();
        }
    }
}
