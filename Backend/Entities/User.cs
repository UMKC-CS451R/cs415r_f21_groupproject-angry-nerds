using System.Text.Json.Serialization;

namespace Backend.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public string Role { get; set; }

        [JsonIgnore]
        public byte[] Salt { get; set; }

        [JsonIgnore]
        public byte[] Pwd { get; set; }
    }
}