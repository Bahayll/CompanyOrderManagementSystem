

namespace CompanyOrderManagement.Application.Logging.Services.Interfaces
{
    public interface ILoggerService
    {
        void Verbose(string message);
        void Fatal(string message);
        void Info(string message);
        void Warn(string message);
        void Debug(string message);
        void Error(string message);
        void LogException(Exception ex, string message = "");
    }
}
