using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using StudyBox.Core.Interfaces;
using StudyBox.Core.Models;

namespace StudyBox.Core.Services
{
    public class DeserializeJsonService : IDeserializeJsonService
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
