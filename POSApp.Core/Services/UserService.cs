using POSApp.Core.Interfaces;
using POSApp.Core.Models;

public class UserService : IUserService
{
    private readonly List<User> _users = new();

    public IEnumerable<User> GetAllUsers() => _users;

    public void AddUser(User user)
    {
        _users.Add(user);
    }

    public void UpdateUser(User user)
    {
        var existing = _users.FirstOrDefault(u => u.Id == user.Id);
        if (existing != null)
        {
            existing.Username = user.Username;
            existing.PasswordHash = user.PasswordHash;
            existing.Role = user.Role;
        }
    }

    public void DeleteUser(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user != null)
            _users.Remove(user);
    }
}