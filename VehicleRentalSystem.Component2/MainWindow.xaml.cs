using System.Windows;
using VehicleRentalSystem.Component2.ViewModels;

namespace VehicleRentalSystem.Component2
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
