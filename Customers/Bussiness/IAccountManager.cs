using Data.Models;

namespace Bussiness
{
    public interface IAccountManager
    {
        User VerifyUserPassword(string userName, string password);
    }
}
