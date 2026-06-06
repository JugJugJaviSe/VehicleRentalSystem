using System;
using System.IO;
using System.Text;
using VehicleRentalSystem.Services.Interfaces;

namespace VehicleRentalSystem.Services.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly string _logFilePath;

        public LoggingService(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public void Log(string message)
        {
            try
            {
                string directory = Path.GetDirectoryName(_logFilePath);

                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (StreamWriter writer = new StreamWriter(
                    _logFilePath,
                    true,
                    new UTF8Encoding(false)))
                {
                    writer.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}");
                }
            }
            catch (Exception)
            {
                // Logging failures must not propagate to the UI layer.
            }
        }
    }
}
