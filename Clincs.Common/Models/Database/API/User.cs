using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Clincs.Common.Models.Database.API
{
    public class User
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }

        public List<Appointment> Appointments { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }
    }
}
