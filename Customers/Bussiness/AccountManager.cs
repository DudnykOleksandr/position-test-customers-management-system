using Data.Models;
using Data.Repositories;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Bussiness
{
    public class AccountManager : IAccountManager
    {
        private readonly ICustomerRepository _customerRepository;
        private const int _saltSize = 6;

        public AccountManager(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public User VerifyUserPassword(string userName, string password)
        {
            User result = null;

            var user = _customerRepository.GetUser(userName);
            if (user != null && IsPasswordMatch(user.PasswordHashSalt, user.PasswordHash, password))
            {
                result = user;
            }

            return result;
        }

        public static bool IsPasswordMatch(string salt, string hashedPassword, string password)
        {
            return (hashedPassword == Hash(salt, password));
        }

        public static string Hash(string salt, string password)
        {
            byte[] inputBytes = Encoding.Unicode.GetBytes(salt + password);
            byte[] outputBytes;

            using (var sha = new SHA256Managed())
            {
                outputBytes = sha.ComputeHash(inputBytes);
            }

            return Convert.ToBase64String(outputBytes);
        }

        public static string GenerateRandomSalt()
        {
            return Convert.ToBase64String(GenerateRandomBytes(_saltSize));
        }

        private static byte[] GenerateRandomBytes(int arrayLength)
        {
            byte[] rndBytes = new byte[arrayLength];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(rndBytes);
            }

            return rndBytes;
        }
    }
}
