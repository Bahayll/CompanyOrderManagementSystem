using CompanyOrderManagement.Application.Logging.ConfigurationModels.Configuration;
using CompanyOrderManagement.Application.Logging.Services.Concretes;
using CompanyOrderManagement.Application.Logging.Services.Messages;
using Microsoft.Extensions.Options;
using Serilog;

namespace CompanyOrderManagement.Application.Logging.Services
{
    public class ConsoleLogger : LoggerService
    {
        public ConsoleLogger(IOptions<LoggingConfiguration> loggingConfiguration)
        {
            try
            {
                var consoleLogConfiguration = loggingConfiguration.Value.ConsoleLogConfiguration;
                Logger = new LoggerConfiguration()
                    .MinimumLevel.Is(consoleLogConfiguration.MinimumLevel)
                    .WriteTo.Console()
                    .CreateLogger();
            }
            catch(Exception ex)
            {
                Console.WriteLine(LoggerExceptionMessages.ConsoleLoggerFailed(ex.Message));
                Logger?.Error(LoggerExceptionMessages.ConsoleLoggerFailed(ex.Message));
            }
           
           
        }
    }
}
