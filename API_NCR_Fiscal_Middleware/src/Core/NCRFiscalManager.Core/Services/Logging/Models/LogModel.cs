namespace NCRFiscalManager.Core.Services.Logging.Models
{
    public class LogModel
    {
        public static string appSettingSectionName = "NCRFiscalLogs";
        public string LogsPath { get; set; }
        public string FileName { get; set; }
    }
}
