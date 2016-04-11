using Newtonsoft.Json;

namespace StudyBox.Core.Models
{
    public class User
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
         
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        public User() { }

        public User(string email, string password)
        {
            this.Email = email;
            this.Password = password;
        }

        public User(string id, string name, string email, string password)
        {
            this.ID = id;
            this.Email = email;
            this.Name = name;
            this.Password = password;
        }
    }
}
