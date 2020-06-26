using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using WindowsServiceWorker.Models;
using WindowsServiceWorker.Service;

namespace WindowsServiceWorker
{
    public class Program
    {
        public static void Main(String[] args)
        {
            var location = System.Reflection.Assembly.GetEntryAssembly().Location;

            var path = Path.GetDirectoryName(location);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                    .WriteTo.File($"{path}/logs/log-.log", rollingInterval: RollingInterval.Day)
                    .CreateLogger();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .UseWindowsService()
               .ConfigureAppConfiguration((hostingContext, config) =>
               {
                   var env = hostingContext.HostingEnvironment;
                   config.SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
               })
               .ConfigureServices((hostContext, services) =>
               {
                   ConfigureServices(hostContext.Configuration, services);
                   services.AddHostedService<Worker>();
               }).UseSerilog();

        private static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddSingleton(configuration.GetSection("BasicConfiguration").Get<BasicConfiguration>());
            services.TryAddSingleton<IWorkerService, WorkerService>();
        }
    }
}