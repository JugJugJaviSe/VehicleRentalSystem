using System.Windows;
using VehicleRentalSystem.Component1.ViewModels;

namespace VehicleRentalSystem.Component1
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
