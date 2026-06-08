using System.Windows;
using VehicleRentalSystem.Component1.ViewModels;

namespace VehicleRentalSystem.Component1.Views
{
    public partial class VehicleDialog : Window
    {
        public VehicleDialog()
            : this(new VehicleDialogViewModel())
        {
        }

        public VehicleDialog(VehicleDialogViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.RequestClose += CloseDialog;
        }

        private void CloseDialog(bool? result)
        {
            DialogResult = result;
            Close();
        }
    }
}
