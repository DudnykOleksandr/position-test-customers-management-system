using Data.Models;
using System.Collections.Generic;

namespace Data.Repositories
{
    public interface ICustomerRepository
    {
        IEnumerable<string> GetAllUserNames();

        User GetUser(string userName);

        IEnumerable<Customer> GetAll(string customerId = "");

        void Save(Customer customer);

        void Delete(Customer customer);

        void CreateUser(User user);
    }
}
