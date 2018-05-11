using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private CustomerDBContext _dbContext;

        public CustomerRepository(CustomerDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<string> GetAllUserNames()
        {
            return _dbContext.User.Select(u => u.UserName).ToList();
        }

        public IEnumerable<Customer> GetAll()
        {
            return _dbContext.Customer
                .Include(c => c.Contacts)
                .Include(c => c.Address)
                .Include(c => c.Departments).ThenInclude(cd => cd.Address)
                .Include(c => c.Users)
                .ToList();
        }

        public void Save(Customer customer)
        {
            if (customer.ActionType == EntityActionType.Add)
            {
                _dbContext.Add(customer);
            }
            else if (customer.ActionType == EntityActionType.Update)
            {
                var existingCustomer = GetCustomerWithRelatedData(customer.CustomerId);
                _dbContext.Entry(existingCustomer).CurrentValues.SetValues(customer);

                ProcessChildEntity(customer.Address, existingCustomer.Address);

                foreach (var contact in customer.Contacts)
                {
                    var existingContact = existingCustomer.Contacts.SingleOrDefault(item => item.ContactId == contact.ContactId);

                    ProcessChildEntity(contact, existingContact);
                }

                foreach (var user in customer.Users)
                {
                    var existingUser = existingCustomer.Users.SingleOrDefault(item => item.UserId == user.UserId);

                    ProcessChildEntity(user, existingUser);
                }

                foreach (var department in customer.Departments)
                {
                    var existingDepartment = existingCustomer.Departments.SingleOrDefault(item => item.DepartmentId == department.DepartmentId);

                    ProcessChildEntity(department.Address, existingDepartment.Address);

                    ProcessChildEntity(department, existingDepartment);
                }
            }

            _dbContext.SaveChanges();
        }

        public void Delete(Customer customer)
        {
            _dbContext.Customer.Attach(customer);
            _dbContext.Address.Attach(customer.Address);
            _dbContext.Customer.Remove(customer);
            _dbContext.Address.Remove(customer.Address);
            _dbContext.SaveChanges();
        }

        private void ProcessChildEntity(Base entity, Base existingEntity)
        {
            if (entity.ActionType == EntityActionType.Add)
                _dbContext.Add(entity);
            else if (entity.ActionType == EntityActionType.Update)
                _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
            else if (entity.ActionType == EntityActionType.Delete)
            {
                _dbContext.Remove(existingEntity);
                if (existingEntity is Department)
                    _dbContext.Remove(((Department)existingEntity).Address);
            }
        }

        private Customer GetCustomerWithRelatedData(Guid customerId)
        {
            var customer = _dbContext.Customer
                .Where(c => c.CustomerId == customerId)
                .Include(c => c.Contacts)
                .Include(c => c.Address)
                .Include(c => c.Departments).ThenInclude(cd => cd.Address)
                .Include(c => c.Users)
                .SingleOrDefault();

            if (customer != null)
                return customer;

            //TODO add specific type of exception
            throw new Exception(string.Format("Customer with Id {0} not found", customerId));
        }
    }
}
