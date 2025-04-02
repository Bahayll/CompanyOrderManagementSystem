

namespace CompanyOrderManagement.Application.Logging.ConfigurationModels.Configuration
{
    public class LoggingConfiguration
    {
        public ConsoleLogConfiguration ConsoleLogConfiguration { get; set; }
        public FileLogConfiguration FileLogConfiguration { get; set; }
        public DatabaseLogConfiguration DatabaseLogConfiguration { get; set; }
    }
}
