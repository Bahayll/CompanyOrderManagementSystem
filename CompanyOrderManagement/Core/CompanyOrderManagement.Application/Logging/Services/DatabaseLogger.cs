using Serilog.Events;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using CompanyOrderManagement.Application.Logging.ConfigurationModels.Configuration;
using Microsoft.Extensions.Options;
using CompanyOrderManagement.Application.Logging.Services.Concretes;
using CompanyOrderManagement.Application.Logging.Services.Messages;

namespace CompanyOrderManagement.Application.Logging.Services
{
    public class DatabaseLogger : LoggerService
    {
        public DatabaseLogger(IOptions<LoggingConfiguration> loggingConfiguration) {

            try
            {
                var dbLoggingConfiguration = loggingConfiguration.Value.DatabaseLogConfiguration;
                Logger = new LoggerConfiguration()
                    .WriteTo.MSSqlServer(
                        connectionString: dbLoggingConfiguration.ConnectionString,
                        tableName: dbLoggingConfiguration.TableName,
                        autoCreateSqlTable: true,
                        columnOptions: new ColumnOptions(),
                        restrictedToMinimumLevel: LogEventLevel.Information
                    )
                    .CreateLogger();
            }
            catch ( Exception ex )
            {
                Console.WriteLine(LoggerExceptionMessages.DatabaseLoggerFailde(ex.Message));
                Logger?.Error(LoggerExceptionMessages.DatabaseLoggerFailde(ex.Message));
            }
         
        }
    }
}
