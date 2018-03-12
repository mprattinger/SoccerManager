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
using Serilog;
using Serilog.Events;

namespace SoccerManager
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
            //.WriteTo.RollingFile(Path.Combine(Environment.CurrentDirectory, "log", "log-{Date}.txt"))
            //.CreateLogger();

            try
            {
                var host = BuildWebHost(args);

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        DbInitializer.Initialize(services);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("An error occurred seeding the DB.", ex);
                    }
                }

                Log.Information("Starting web host");
                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly", ex);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }

            //BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog()
                .Build();
    }
}
