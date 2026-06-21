namespace VehicleRentalSystem.Component2.Interfaces
{
    public interface ISaveFileDialogService
    {
        string GetSaveFilePath(string filter, string defaultExtension, string suggestedFileName);
    }
}
