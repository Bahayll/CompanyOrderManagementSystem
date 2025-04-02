
namespace CompanyOrderManagement.Application.Logging.Services.Concretes
{
    public class CompositeLogger : LoggerService
    {
        private readonly ConsoleLogger _consoleLogger;
        private readonly FileLogger _fileLogger;
        private readonly DatabaseLogger _databaseLogger;

        public CompositeLogger(ConsoleLogger consoleLogger, FileLogger fileLogger, DatabaseLogger databaseLogger)
        {
            _consoleLogger = consoleLogger;
            _fileLogger = fileLogger;
            _databaseLogger = databaseLogger;
        }

        public override void Verbose(string message)
        {
            _consoleLogger.Verbose(message);
            _fileLogger.Verbose(message);
            _databaseLogger.Verbose(message);
        }

        public override void Fatal(string message)
        {
            _consoleLogger.Fatal(message);
            _fileLogger.Fatal(message);
            _databaseLogger.Fatal(message);
        }

        public override void Info(string message)
        {
            _consoleLogger.Info(message);
            _fileLogger.Info(message);
            _databaseLogger.Info(message);
        }

        public override void Warn(string message)
        {
            _consoleLogger.Warn(message);
            _fileLogger.Warn(message);
            _databaseLogger.Warn(message);
        }

        public override void Debug(string message)
        {
            _consoleLogger.Debug(message);
            _fileLogger.Debug(message);
            _databaseLogger.Debug(message);
        }

        public override void Error(string message)
        {
            _consoleLogger.Error(message);
            _fileLogger.Error(message);
            _databaseLogger.Error(message);
        }

        public override void LogException(Exception ex, string message = "")
        {
            _consoleLogger.LogException(ex, message);
            _fileLogger.LogException(ex, message);
            _databaseLogger.LogException(ex, message);
        }
    }
}
