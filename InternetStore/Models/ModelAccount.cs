using InternetStore.Enums;
using System.Data;

namespace InternetStore.Models
{
    public class ModelAccount
    {
        public string Id { get; set; } = "";
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public UserRole Role { get; set; }
        public string Password { get; set; }
    }
}
