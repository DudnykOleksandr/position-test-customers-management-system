using Data.Models;

namespace Data.Repositories
{
    public class CustomerRepository
    {
        private CustomerDBContext dbContext;

        public void SaveCustomer(Customer customer)
        {
            dbContext.Add(customer);
            dbContext.SaveChanges();
        }
    }
}
