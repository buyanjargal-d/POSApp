using POSApp.Core.Models;
using POSApp.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace POSApp.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly List<User> _users;

        public AuthService()
        {
            _users = new List<User>
            {
                new User { Id = 1, Username = "manager", PasswordHash = "manager123", Role = Role.Manager },
                new User { Id = 2, Username = "cashier1", PasswordHash = "cashier123", Role = Role.Cashier },
                new User { Id = 3, Username = "cashier2", PasswordHash = "cashier123", Role = Role.Cashier }
            };
        }

        public User Login(string username, string password)
        {
            return _users.FirstOrDefault(u => u.Username == username && u.PasswordHash == password);
        }
    }
}