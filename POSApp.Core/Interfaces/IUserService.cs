using System.Collections.Generic;
using POSApp.Core.Models;

namespace POSApp.Core.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
    }
}