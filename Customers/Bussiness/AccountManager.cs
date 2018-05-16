using Data.Models;
using Data.Repositories;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Bussiness
{
    /// <summary>
    /// Contains all user password hash comparing, generating logic
    /// </summary>
    public class AccountManager : IAccountManager
    {
        private const int _saltSize = 6;
        private readonly ICustomerRepository _customerRepository;

        public AccountManager(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// Computes hash from password and salt
        /// </summary>
        /// <param name="salt"></param>
        /// <param name="password"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Generates random salt string
        /// </summary>
        /// <returns></returns>
        public static string GenerateRandomSalt()
        {
            return Convert.ToBase64String(GenerateRandomBytes(_saltSize));
        }

        /// <summary>
        /// Verifies user name and password. Returns user if verification was successful
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Matches password and salt with hashed password
        /// </summary>
        /// <param name="salt"></param>
        /// <param name="hashedPassword"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool IsPasswordMatch(string salt, string hashedPassword, string password)
        {
            return hashedPassword == Hash(salt, password);
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
