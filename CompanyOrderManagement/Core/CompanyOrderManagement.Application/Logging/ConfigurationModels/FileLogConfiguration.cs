
namespace CompanyOrderManagement.Application.Logging.ConfigurationModels
{
    public class FileLogConfiguration
    {
        public string FolderPath { get; set; }
        public int RetainedFileCountLimit { get; set; }
        public long FileSizeLimitBytes { get; set; }
        public string FileName { get; set; }
    }
}
