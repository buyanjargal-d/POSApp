using Xunit;
using POSApp.Core.Services;
using POSApp.Core.Models;

namespace POSApp.Core.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _authService = new AuthService();
        }

        [Fact]
        public void Login_ValidCredentials_ReturnsUser()
        {
            var username = "manager";
            var password = "manager123";

            var user = _authService.Login(username, password);

            Assert.NotNull(user);
            Assert.Equal(1, user.Id);
            Assert.Equal("manager", user.Username);
            Assert.Equal(Role.Manager, user.Role);
        }

        [Theory]
        [InlineData("invalid", "password")]
        [InlineData("manager", "wrongpass")]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void Login_InvalidCredentials_ReturnsNull(string username, string password)
        {
            var user = _authService.Login(username, password);

            Assert.Null(user);
        }
    }
}
