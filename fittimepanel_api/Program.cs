using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //09AQbkAaouq81ybDrenS
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var isDevelopment = environment == Environments.Development;

            if (isDevelopment)
            {
                Log.Logger = new LoggerConfiguration()
                    .WriteTo.File(
                        path: "c:\\DevLogs\\Fittime\\log-.txt",
                        rollingInterval: RollingInterval.Day,
                        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                    .CreateLogger();
            }
            else
            {
                var seqKey = Environment.GetEnvironmentVariable("SEQ_KEY");
                var levelSwitch = new LoggingLevelSwitch();

                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.ControlledBy(levelSwitch)
                    .WriteTo.Seq("http://localhost:5341",
                                 apiKey: seqKey,
                                 controlLevelSwitch: levelSwitch)
                    .CreateLogger();
            }

            try
            {
                Log.Information("Service is starting...");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Something happened in start");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

    }
}
