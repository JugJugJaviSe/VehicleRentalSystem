using System;
using System.IO;

namespace VehicleRentalSystem.Services.Services
{
    public class LoggingService
    {
        private const string LogFileName = "ActivityLog.txt";

        public void Log(string message)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(LogFileName, true))
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
