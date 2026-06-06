using System.Windows;
using VehicleRentalSystem.Models.Enums;

namespace VehicleRentalSystem.Component1.Views
{
    public partial class VehicleDialog : Window
    {
        public string LicensePlate
        {
            get => LicensePlateTextBox.Text;
            set => LicensePlateTextBox.Text = value;
        }

        public string Brand
        {
            get => BrandTextBox.Text;
            set => BrandTextBox.Text = value;
        }

        public string Model
        {
            get => ModelTextBox.Text;
            set => ModelTextBox.Text = value;
        }

        public int ProductionYear
        {
            get => int.TryParse(ProductionYearTextBox.Text, out int year) ? year : 0;
            set => ProductionYearTextBox.Text = value.ToString();
        }

        public FuelType FuelType
        {
            get => FuelTypeComboBox.SelectedItem is FuelType fuelType ? fuelType : default;
            set => FuelTypeComboBox.SelectedItem = value;
        }

        public VehicleDialog()
        {
            InitializeComponent();
            FuelTypeComboBox.SelectedIndex = 0;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
