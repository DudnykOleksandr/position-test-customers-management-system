using Data.Models;
using System.Collections.Generic;

namespace Data.Repositories
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAllCustomers();

        void SaveCustomer(Customer customer);
    }
}
