using InternetStore.Enums;
using InternetStore.Models;

namespace TestProject
{
    public class UserTest
    {
        [Fact]
        public void TestUser()
        {
            User user = new()
            {
                Id = "8fyhj378jf891kf",
                Name = "User",
                UserName = "user1",
                Email = "user1@gmail.com",
                PhoneNumber = "0990000000",
                Role = UserRole.User
            };
            Assert.Equal("8fyhj378jf891kf", user.Id);
            Assert.Equal("User", user.Name);
            Assert.Equal("user1", user.UserName);
            Assert.Equal("user1@gmail.com", user.Email);
            Assert.Equal("0990000000", user.PhoneNumber);
            Assert.Equal(UserRole.User, user.Role);
        }
    }
}