using System.Linq;
using System.Windows;
using VehicleRentalSystem.Component1.ViewModels;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component1.Views
{
    public partial class RentalRecordDialog : Window
    {
        public RentalRecordDialog()
            : this(new RentalRecordDialogViewModel(Enumerable.Empty<Vehicle>()))
        {
        }

        public RentalRecordDialog(RentalRecordDialogViewModel viewModel)
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
