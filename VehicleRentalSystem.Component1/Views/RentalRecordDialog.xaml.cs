using System;
using System.Collections.Generic;
using System.Windows;
using VehicleRentalSystem.Models.Enums;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component1.Views
{
    public partial class RentalRecordDialog : Window
    {
        public IEnumerable<Vehicle> Vehicles
        {
            set
            {
                VehicleComboBox.ItemsSource = value;
                VehicleComboBox.SelectedIndex = 0;
            }
        }

        public Guid SelectedVehicleId
        {
            get => VehicleComboBox.SelectedItem is Vehicle vehicle ? vehicle.Id : Guid.Empty;
            set => VehicleComboBox.SelectedValue = value;
        }

        public DateTime RentalDate
        {
            get => RentalDatePicker.SelectedDate ?? default;
            set => RentalDatePicker.SelectedDate = value;
        }

        public double DurationDays
        {
            get => double.TryParse(DurationDaysTextBox.Text, out double value) ? value : 0;
            set => DurationDaysTextBox.Text = value.ToString();
        }

        public double TotalCost
        {
            get => double.TryParse(TotalCostTextBox.Text, out double value) ? value : 0;
            set => TotalCostTextBox.Text = value.ToString();
        }

        public double MileageDriven
        {
            get => double.TryParse(MileageDrivenTextBox.Text, out double value) ? value : 0;
            set => MileageDrivenTextBox.Text = value.ToString();
        }

        public RentalState SelectedState
        {
            get => StateComboBox.SelectedItem is RentalState state ? state : default;
            set => StateComboBox.SelectedItem = value;
        }

        public RentalRecordDialog()
        {
            InitializeComponent();
            VehicleComboBox.SelectedValuePath = nameof(Vehicle.Id);
            StateComboBox.ItemsSource = Enum.GetValues(typeof(RentalState));
            StateComboBox.SelectedIndex = 0;
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
