namespace VehicleRentalSystem.Component2.Interfaces
{
    public interface IRentalRecordsSubject
    {
        void Attach(IRentalRecordsObserver observer);
        void Detach(IRentalRecordsObserver observer);
        void NotifyObservers();
    }
}
