using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VehicleRentalSystem.Component1.ViewModels;
using VehicleRentalSystem.Services.Interfaces;
using VehicleRentalSystem.Services.Services;

namespace VehicleRentalSystem.Component1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string dataDirectory = System.IO.Path.Combine(
                System.AppDomain.CurrentDomain.BaseDirectory,
                "Data");
            ILoggingService loggingService = new LoggingService(
                System.IO.Path.Combine(dataDirectory, "activity.log"));

            MainWindow mainWindow = new MainWindow(new MainViewModel(loggingService));
            mainWindow.Show();
        }
    }
}
