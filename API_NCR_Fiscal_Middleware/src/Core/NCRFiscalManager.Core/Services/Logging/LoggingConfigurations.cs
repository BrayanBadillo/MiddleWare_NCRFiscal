using Microsoft.Extensions.Configuration;
using NCRFiscalManager.Core.Interfaces.Logging;
using NCRFiscalManager.Core.Services.Logging.Models;
using Serilog;

namespace NCRFiscalManager.Core.Services.Logging
{
    public class LoggingConfigurations : ILoggingConfigurations
    {
        private readonly IConfiguration _configuration;

        public LoggingConfigurations(IConfiguration configuration) => _configuration = configuration;
        public void ConfigureLogs()
        {
            LogModel logModel = _configuration
                .GetSection(LogModel.appSettingSectionName)
                .Get<LogModel>();

            if (!Directory.Exists(logModel.LogsPath))
            {
                try
                {
                    Directory.CreateDirectory(logModel.LogsPath);
                }
                catch (Exception e)
                {
                    throw new Exception($"Error trying to create log directory [{logModel.LogsPath}]!  {e.Message}");
                }
            }
            string serilogPathFile = Path.Combine(logModel.LogsPath, logModel.FileName);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File(serilogPathFile, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message}{NewLine}{Exception}", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 90, shared: true)
                .CreateLogger();
        }
    }
}
