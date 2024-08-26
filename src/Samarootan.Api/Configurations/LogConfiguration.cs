using Serilog;
using Serilog.Events;
using Middlewares;

namespace Samarootan_Api.Configuration
{
    public  static class LogConfiguration
    {
            public static void UseLogConfiguration(this IApplicationBuilder builder)
            {
                Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Debug()
           .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
           .Enrich.FromLogContext()
           .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
           .WriteTo.File("logs/microservice-template.txt",
               rollingInterval: RollingInterval.Day,
               outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();
            }
        
    }
    
}
