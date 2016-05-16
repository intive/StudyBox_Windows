using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Storage;
using StudyBox.Core.Interfaces;
using Windows.Storage.Pickers;

namespace StudyBox.Core.Services
{
    public class CameraService : ICameraService
    {
        public async Task<bool> IsCamera()
        {
            DeviceInformationCollection videoDevices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            if (videoDevices.Count < 1)
                return false;
            else
                return true;
        }

        public async Task<StorageFile> CapturePhoto()
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.AllowCropping = false;
            captureUI.PhotoSettings.MaxResolution = CameraCaptureUIMaxPhotoResolution.HighestAvailable;
            StorageFile image = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (image != null)
                await image.RenameAsync("New deck " + DateTime.Now.ToString("yyyyMMdd Hmmss") + ".jpg");
            return image;
        }

        public async Task<StorageFile> PickPhoto()
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".txt");
            return await openPicker.PickSingleFileAsync();
        }
    }
}
