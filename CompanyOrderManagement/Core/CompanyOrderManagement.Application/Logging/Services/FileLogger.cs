using CompanyOrderManagement.Application.Logging.ConfigurationModels.Configuration;
using CompanyOrderManagement.Application.Logging.Services.Concretes;
using CompanyOrderManagement.Application.Logging.Services.Messages;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Formatting.Compact;


namespace CompanyOrderManagement.Application.Logging.Services
{
    public class FileLogger : LoggerService
    {
        public FileLogger(IOptions<LoggingConfiguration> loggingConfiguration) 
        {
            try
            {
                var fileLogConfiguration = loggingConfiguration.Value.FileLogConfiguration;

                Logger = new LoggerConfiguration()
                    .WriteTo.File(
                        new CompactJsonFormatter(),
                        Path.Combine(fileLogConfiguration.FolderPath, fileLogConfiguration.FileName),
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: fileLogConfiguration.RetainedFileCountLimit,
                        fileSizeLimitBytes: fileLogConfiguration.FileSizeLimitBytes
                        )
                    .CreateLogger();
            }
            catch ( Exception ex )
            {
                Console.WriteLine(LoggerExceptionMessages.FileLoggerFailed(ex.Message));
                Logger?.Error(LoggerExceptionMessages.FileLoggerFailed(ex.Message));
            }
        } 
    }
}
