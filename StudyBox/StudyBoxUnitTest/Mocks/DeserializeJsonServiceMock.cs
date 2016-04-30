using System;
using Newtonsoft.Json;
using StudyBox.Core.Interfaces;

namespace StudyBoxUnitTest.Mocks
{
    public class DeserializeJsonServiceMock : IDeserializeJsonService
    {
        public T GetObjectFromJson<T>(string jsonToDeserialize)
        {
            if (!String.IsNullOrEmpty(jsonToDeserialize))
            {
                try
                {
                    T tagObject = JsonConvert.DeserializeObject<T>(jsonToDeserialize);
                    return tagObject;
                }
                catch (Exception)
                {
                    return default(T);
                }
            }
            else
                return default(T);
        }
    }
}