using InternetStore.Data;
using InternetStore.Models;
using InternetStore.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InternetStore.Classes
{
    public class DefaultAdmin
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
                context.Database.EnsureCreated();
                if (await userManager.FindByNameAsync("admin") == null)
                {
                    var admin = new User
                    {
                        UserName = "admin",
                        Email = "admin@example.com",
                        PhoneNumber = "0990000000",
                        Name = "Admin",
                        Role = UserRole.Admin
                    };
                    var result = await userManager.CreateAsync(admin, "admin12345");
                }
            }
        }
    }
}
