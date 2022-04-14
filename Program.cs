using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Serilog;

namespace TaskScheduler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                throw new StatusCodeException(HttpStatusCode.ServiceUnavailable, ex.Message);
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, services, configuration) => configuration
                        .ReadFrom.Configuration(context.Configuration)
                        .ReadFrom.Services(services)
                        .Enrich.FromLogContext()
#if DEBUG
                        .WriteTo.Console()
#endif
                )
                .ConfigureHostConfiguration(configHost =>
                {

                    configHost.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);

                    configHost.AddEnvironmentVariables();
                    configHost.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    configHost.AddJsonFile(
                        $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                        optional: true, reloadOnChange: true);

                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                    webBuilder.UseIISIntegration();

                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseContentRoot(AppContext.BaseDirectory);
                });
    }
}

