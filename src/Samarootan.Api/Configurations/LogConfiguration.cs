namespace Samarootan.Api.Configurations
{
    using Serilog;
    using Serilog.Events;
    using Serilog.Sinks.SystemConsole.Themes;

    /// <summary>
    /// Represents the log configuration.
    /// </summary>
    public static class LogConfiguration
    {
        /// <summary>
        /// Initializes the log configuration.
        /// </summary>
        public static void Initialize()
        {
            const string tamplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

            Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                            .Enrich.FromLogContext()
                            .WriteTo.Console(outputTemplate: tamplate, theme: AnsiConsoleTheme.Sixteen)
                            .WriteTo.File(
                                "logs/microservice-template.txt",
                                rollingInterval: RollingInterval.Day,
                                outputTemplate: tamplate)
                            .CreateLogger();
        }
    }
}
