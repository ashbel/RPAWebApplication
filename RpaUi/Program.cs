using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;

namespace RpaUi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseSentry(options =>
                    {
                        options.Release = "1.10";

                        options.MaxBreadcrumbs = 200;

                        options.EnableTracing = true;

                        options.DecompressionMethods = DecompressionMethods.None;

                        options.MaxQueueItems = 100;

                        options.ShutdownTimeout = TimeSpan.FromSeconds(5);

                        options.ConfigureScope(s => s.SetTag("Always sent", "this tag"));
                    });
                });
    }
}
