using Microsoft.Win32;
using VehicleRentalSystem.Component2.Interfaces;

namespace VehicleRentalSystem.Component2.Services
{
    public class SaveFileDialogService : ISaveFileDialogService
    {
        public string GetSaveFilePath(string filter, string defaultExtension, string suggestedFileName)
        {
            var dialog = new SaveFileDialog
            {
                Filter = filter,
                DefaultExt = defaultExtension,
                FileName = suggestedFileName
            };

            var result = dialog.ShowDialog();
            return result == true ? dialog.FileName : null;
        }
    }
}
