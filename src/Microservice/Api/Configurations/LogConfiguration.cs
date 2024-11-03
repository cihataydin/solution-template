namespace Microservice.Api.Configurations
{
    using Microservice.Domain.Constants;
    using Serilog;
    using Serilog.Events;
    using Serilog.Formatting.Json;
    using Serilog.Sinks.SystemConsole.Themes;

    /// <summary>
    /// Represents the log configuration.
    /// </summary>
    public static class LogConfiguration
    {
        /// <summary>
        /// Initializes the log configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static void Initialize(ConfigurationManager configuration)
        {
            const string tamplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

            bool useJsonFormat = configuration.GetValue<bool>(ConfigurationConstant.UseJsonFormat);
            var loggerConfiguration = new LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                            .Enrich.FromLogContext()
                            .WriteTo.File(
                                "logs/microservice-template.txt",
                                rollingInterval: RollingInterval.Day,
                                outputTemplate: tamplate);

            if (useJsonFormat)
            {
                loggerConfiguration.WriteTo.Console(new JsonFormatter())
                .WriteTo.File(new JsonFormatter(), "logs/microservice-template.txt", rollingInterval: RollingInterval.Day);
            }
            else
            {
                loggerConfiguration.WriteTo.Console(outputTemplate: tamplate, theme: AnsiConsoleTheme.Sixteen)
                .WriteTo.File("logs/microservice-template.txt", rollingInterval: RollingInterval.Day, outputTemplate: tamplate);
            }

            Log.Logger = loggerConfiguration.CreateLogger();
        }
    }
}
