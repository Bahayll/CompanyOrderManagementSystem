using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Application.Logging.Services.Messages
{
    public static class LoggerExceptionMessages
    {
        public static string ConsoleLoggerFailed(string message) => $"ConsoleLogger initialization failed: {message}";
        public static string FileLoggerFailed(string message) => $"FileLogger initialization failed: {message}";
        public static string DatabaseLoggerFailde(string message) => $"DatabaseLogger initialization failed: {message}";
    }
}
