using System.Collections.Generic;

namespace VehicleRentalSystem.Component1.Observers
{
    public class RentalStatisticsSubject : IRentalSubject
    {
        private readonly List<IRentalObserver> _observers = new List<IRentalObserver>();

        public void RegisterObserver(IRentalObserver observer)
        {
            if (observer != null && !_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void UnregisterObserver(IRentalObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (IRentalObserver observer in _observers)
            {
                observer.Update();
            }
        }
    }
}
