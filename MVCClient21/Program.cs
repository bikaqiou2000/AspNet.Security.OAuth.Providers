using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVCClient21.Extensions;
using NLog.Web;

namespace MVCClient21
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // NLog: setup the logger first to catch all errors
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            var host = CreateWebHostBuilder(args).Build();
            //var logger = host.Services.GetRequiredService<ILogger<Program>>();
            //logger.LogInformation("Main Start.");

            logger.Info("Main");

            host.Run();
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

            //return WebHost.CreateDefaultBuilder(args)
            //    //.ConfigureAppConfiguration((hostingContext, config) =>
            //    //{
            //    //    config.SetBasePath(Directory.GetCurrentDirectory());
            //    //    config.AddJsonFile("hosting.json", optional: false, reloadOnChange: false);
            //    //    config.AddCommandLine(args);
            //    //})
            //    //.UseConfiguration(config)
            //    //.UseUrls("http://localhost:60000", "https://localhost:60001")
            //    //.UseKestrel()
            //    .UseStartup<Startup>();

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", true, true)
                .Build();

            var urlstr = config.GetSection("server.urls").Value;
            var host = WebHost.CreateDefaultBuilder(args)
               .ConfigureLogging(logging =>
               {
                   logging.ClearProviders();
                   logging.SetMinimumLevel(LogLevel.Trace);
                   logging.AddConsole();
               })
               .UseNLog() 
               .UseStartup<Startup>();
            if (!string.IsNullOrEmpty(urlstr))
            {
                host.UseUrls(urlstr);
            }

           

            return host;


        }
    }
}
