using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using Serilog;


namespace CompanyOrderManagement.Application.Logging.Services.Concretes
{
    public abstract class LoggerService : ILoggerService
    {
        protected ILogger Logger { get; set; }

        protected LoggerService()
        {
            Logger = Log.Logger;
        }

        public virtual void Verbose(string message) => Logger.Verbose(message);
        public virtual void Fatal(string message) => Logger.Fatal(message);
        public virtual void Info(string message) => Logger.Information(message);
        public virtual void Warn(string message) => Logger.Warning(message);
        public virtual void Debug(string message) => Logger.Debug(message);
        public virtual void Error(string message) => Logger.Error(message);

        public virtual void LogException(Exception ex, string message = "")
        {
            Logger.Error(ex, message);
        }
    }
}
