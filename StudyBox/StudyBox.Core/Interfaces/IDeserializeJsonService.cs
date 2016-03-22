using StudyBox.Core.Models;
using System.Collections.Generic;

namespace StudyBox.Core.Interfaces
{
    public interface IDeserializeJsonService
    {
        T GetObjectFromJson<T>(string jsonToDeserialize);
    }
}
