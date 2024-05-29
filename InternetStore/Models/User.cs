using InternetStore.Enums;
using Microsoft.AspNetCore.Identity;

namespace InternetStore.Models
{
    public class User: IdentityUser
    {
        public string Name { get; set; }
        public UserRole Role { get; set; }
    }
}
