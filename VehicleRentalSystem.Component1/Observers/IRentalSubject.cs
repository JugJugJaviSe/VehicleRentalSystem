namespace VehicleRentalSystem.Component1.Observers
{
    public interface IRentalSubject
    {
        void RegisterObserver(IRentalObserver observer);
        void UnregisterObserver(IRentalObserver observer);
        void NotifyObservers();
    }
}
