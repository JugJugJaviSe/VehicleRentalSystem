using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VehicleRentalSystem.Component2.Interfaces;
using VehicleRentalSystem.Component2.Models;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component2.ViewModels
{
    public class RentalStatisticsViewModel : BaseViewModel, IRentalRecordsObserver
    {
        private readonly IReadOnlyList<IRentalStatisticStrategy> _strategies;
        private IReadOnlyList<RentalRecord> _currentRecords = new List<RentalRecord>();
        private IRentalStatisticStrategy _selectedStrategy;
        private string _selectedStatisticResult;

        public IReadOnlyList<IRentalStatisticStrategy> AvailableStrategies => _strategies;

        public ObservableCollection<StatisticResult> Statistics { get; }
            = new ObservableCollection<StatisticResult>();

        public IRentalStatisticStrategy SelectedStrategy
        {
            get => _selectedStrategy;
            set
            {
                _selectedStrategy = value;
                OnPropertyChanged();
                CalculateSelectedStatistic();
            }
        }

        public string SelectedStatisticResult
        {
            get => _selectedStatisticResult;
            set { _selectedStatisticResult = value; OnPropertyChanged(); }
        }

        public RentalStatisticsViewModel(IEnumerable<IRentalStatisticStrategy> strategies)
        {
            _strategies = strategies.ToList();

            if (_strategies.Count > 0)
                SelectedStrategy = _strategies[0];
        }

        public void OnRentalRecordsLoaded(IReadOnlyList<RentalRecord> records)
        {
            _currentRecords = records;

            Statistics.Clear();
            foreach (var strategy in _strategies)
                Statistics.Add(strategy.Calculate(records));

            CalculateSelectedStatistic();
        }

        private void CalculateSelectedStatistic()
        {
            if (_selectedStrategy == null)
            {
                SelectedStatisticResult = "No strategy selected.";
                return;
            }

            if (_currentRecords.Count == 0)
            {
                SelectedStatisticResult = "There are no rental records.";
                return;
            }

            var result = _selectedStrategy.Calculate(_currentRecords);
            SelectedStatisticResult = $"{result.Label}: {result.Value}";
        }
    }
}
