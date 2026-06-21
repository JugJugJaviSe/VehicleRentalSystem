using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using VehicleRentalSystem.Component2.Commands;
using VehicleRentalSystem.Component2.Interfaces;
using VehicleRentalSystem.Component2.Models;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component2.ViewModels
{
    public class RentalStatisticsViewModel : BaseViewModel, IRentalRecordsObserver
    {
        private readonly IReadOnlyList<IRentalStatisticStrategy> _strategies;
        private readonly IRentalStatisticExportService _exportService;
        private readonly ISaveFileDialogService _saveFileDialogService;
        private IReadOnlyList<RentalRecord> _currentRecords = new List<RentalRecord>();
        private IRentalStatisticStrategy _selectedStrategy;
        private string _selectedStatisticResult;
        private string _exportStatusMessage;
        private StatisticResult _lastResult;
        private DateTime _lastCalculatedAt;

        public IReadOnlyList<IRentalStatisticStrategy> AvailableStrategies => _strategies;

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

        public string ExportStatusMessage
        {
            get => _exportStatusMessage;
            set { _exportStatusMessage = value; OnPropertyChanged(); }
        }

        public ICommand ExportStatisticCommand { get; }

        public RentalStatisticsViewModel(
            IEnumerable<IRentalStatisticStrategy> strategies,
            IRentalStatisticExportService exportService,
            ISaveFileDialogService saveFileDialogService)
        {
            _strategies = strategies.ToList();
            _exportService = exportService;
            _saveFileDialogService = saveFileDialogService;
            ExportStatisticCommand = new RelayCommand(_ => ExportStatistic());

            if (_strategies.Count > 0)
                SelectedStrategy = _strategies[0];
        }

        public void OnRentalRecordsLoaded(IReadOnlyList<RentalRecord> records)
        {
            _currentRecords = records;
            CalculateSelectedStatistic();
        }

        private void CalculateSelectedStatistic()
        {
            if (_selectedStrategy == null)
            {
                _lastResult = null;
                SelectedStatisticResult = "No strategy selected.";
                return;
            }

            if (_currentRecords.Count == 0)
            {
                _lastResult = null;
                SelectedStatisticResult = "There are no rental records.";
                return;
            }

            var result = _selectedStrategy.Calculate(_currentRecords);
            _lastResult = result;
            _lastCalculatedAt = DateTime.Now;
            SelectedStatisticResult = $"{result.Label}: {result.Value}";
        }

        private void ExportStatistic()
        {
            if (_lastResult == null)
            {
                ExportStatusMessage = "There is no statistic result to export.";
                return;
            }

            var filePath = _saveFileDialogService.GetSaveFilePath(
                "CSV files (*.csv)|*.csv",
                "csv",
                "statistic-result.csv");

            if (string.IsNullOrEmpty(filePath))
            {
                ExportStatusMessage = "Export cancelled.";
                return;
            }

            try
            {
                var data = new StatisticExportData
                {
                    StatisticName = _lastResult.Label,
                    Value = _lastResult.Value,
                    CalculatedAt = _lastCalculatedAt,
                    RecordsCount = _currentRecords.Count
                };

                _exportService.Export(filePath, data);
                ExportStatusMessage = "Statistic result exported successfully.";
            }
            catch (Exception ex)
            {
                ExportStatusMessage = $"Export failed: {ex.Message}";
            }
        }
    }
}
