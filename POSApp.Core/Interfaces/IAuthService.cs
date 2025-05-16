using POSApp.Core.Models;

namespace POSApp.Core.Interfaces
{
    public interface IAuthService
    {
        User Login(string username, string password);
    }
}