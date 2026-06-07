using System.Collections.Generic;
using System.Linq;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using VehicleRentalSystem.Component1.Observers;
using VehicleRentalSystem.Models.Enums;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component1.ViewModels
{
    public class StatisticsViewModel : BaseViewModel, IRentalObserver
    {
        private readonly IEnumerable<RentalRecord> _rentalRecords;
        private int _activeCount;
        private int _completedCount;
        private int _cancelledCount;
        private int _overdueCount;
        private IEnumerable<ISeries> _series;

        public int ActiveCount
        {
            get => _activeCount;
            private set
            {
                _activeCount = value;
                OnPropertyChanged();
            }
        }

        public int CompletedCount
        {
            get => _completedCount;
            private set
            {
                _completedCount = value;
                OnPropertyChanged();
            }
        }

        public int CancelledCount
        {
            get => _cancelledCount;
            private set
            {
                _cancelledCount = value;
                OnPropertyChanged();
            }
        }

        public int OverdueCount
        {
            get => _overdueCount;
            private set
            {
                _overdueCount = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<ISeries> Series
        {
            get => _series;
            private set
            {
                _series = value;
                OnPropertyChanged();
            }
        }

        public StatisticsViewModel(IEnumerable<RentalRecord> rentalRecords)
        {
            _rentalRecords = rentalRecords;
            Update();
        }

        public void Update()
        {
            ActiveCount = _rentalRecords.Count(record => record.State == RentalState.Active);
            CompletedCount = _rentalRecords.Count(record => record.State == RentalState.Completed);
            CancelledCount = _rentalRecords.Count(record => record.State == RentalState.Cancelled);
            OverdueCount = _rentalRecords.Count(record => record.State == RentalState.Overdue);

            Series = new ISeries[]
            {
                new PieSeries<int> { Name = "Active", Values = new[] { ActiveCount } },
                new PieSeries<int> { Name = "Completed", Values = new[] { CompletedCount } },
                new PieSeries<int> { Name = "Cancelled", Values = new[] { CancelledCount } },
                new PieSeries<int> { Name = "Overdue", Values = new[] { OverdueCount } }
            };
        }
    }
}
