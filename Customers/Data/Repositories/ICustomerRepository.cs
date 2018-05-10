using Data.Models;
using System;
using System.Collections.Generic;

namespace Data.Repositories
{
    public interface ICustomerRepository
    {
        IEnumerable<string> GetAllUserNames();

        IEnumerable<Customer> GetAll();

        void Save(Customer customer);

        void Delete(Guid customerId);
    }
}
