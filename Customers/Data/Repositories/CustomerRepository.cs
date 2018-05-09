using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private CustomerDBContext _dbContext;

        public CustomerRepository(CustomerDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _dbContext.Customer
                .Include(c => c.Contacts)
                .Include(c => c.Address)
                .Include(c => c.Departments)
                .Include(c => c.Users)
                .ToList();
        }

        public void SaveCustomer(Customer customer)
        {
            _dbContext.Add(customer);
            _dbContext.SaveChanges();
        }
    }
}
