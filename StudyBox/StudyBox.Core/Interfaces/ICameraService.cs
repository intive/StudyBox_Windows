using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace StudyBox.Core.Interfaces
{
    public interface ICameraService
    {
        Task<bool> IsCamera();
        Task<StorageFile> CapturePhoto();
        Task<StorageFile> PickPhoto();
    }
}
