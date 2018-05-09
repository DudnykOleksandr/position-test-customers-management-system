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

        public IEnumerable<Customer> GetAll()
        {
            return _dbContext.Customer
                .Include(c => c.Contacts)
                .Include(c => c.Address)
                .Include(c => c.Departments)
                .Include(c => c.Users)
                .ToList();
        }

        public void Save(Customer customer)
        {
            //Customer customerToSave = null;
            //bool isNewCustomer = true;
            //if (customer.Id == Guid.Empty)
            //{
            //    customer.Id = Guid.NewGuid();
            //    _context.Customer.Add(customer);
            //    customerToSave = new Customer();
            //    customerToSave.Name = customer.Name;
            //}
            //else
            //{
            //    isNewCustomer = false;
            //    customerToSave = _context.Customer.Where(c => c.Id == customer.Id).Include(c => c.Contacts).Single();
            //    //customerToSave.Name = customer.Name;
            //    _context.Entry(customerToSave).CurrentValues.SetValues(customer);
            //}

            //foreach (var contact in customer.Contacts)
            //{
            //    if (contact.Id == Guid.Empty)
            //    {
            //        contact.Id = Guid.NewGuid();
            //        customerToSave.Contacts.Add(contact);
            //        _context.Contact.Add(contact);
            //    }
            //    else
            //    {
            //        var existingContact = _context.Contact.Where(c => c.Id == contact.Id).Single();
            //        //existingContact.Name = contact.Name;
            //        _context.Entry(existingContact).CurrentValues.SetValues(contact);
            //    }
            //}
            //if (!isNewCustomer)
            //{
            //    var contactIds = customer.Contacts.Select(c => c.Id);
            //    var contactsToRemove = customerToSave.Contacts.Where(c => !contactIds.Contains(c.Id));
            //    foreach (var contactToRemove in contactsToRemove)
            //        _context.Contact.Remove(contactToRemove);
            //}

            if (customer.ActionType == EntityActionType.Add)
            {
                _dbContext.Add(customer);
            }
            if (customer.ActionType == EntityActionType.Update)
            {
                var existingCustomer = GetCustomerWithRelatedData(customer.CustomerId);
                _dbContext.Entry(existingCustomer).CurrentValues.SetValues(customer);

                ProcessChildEntity(customer.Address, existingCustomer.Address);
            }

            _dbContext.SaveChanges();
        }

        public void Delete(Guid customerId)
        {
            var employer = new Customer { CustomerId = customerId };
            _dbContext.Customer.Attach(employer);
            _dbContext.Customer.Remove(employer);
            _dbContext.SaveChanges();
        }

        private void ProcessChildEntity(Base entity, Base existingEntity)
        {
            if (entity.ActionType == EntityActionType.Add)
            {
                _dbContext.Add(entity);
            }
            if (entity.ActionType == EntityActionType.Update)
            {
                _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
            }
        }

        private Customer GetCustomerWithRelatedData(Guid customerId)
        {
            var customer = _dbContext.Customer
                .Where(c => c.CustomerId == customerId)
                .Include(c => c.Contacts)
                .Include(c => c.Address)
                .Include(c => c.Departments)
                .Include(c => c.Users)
                .SingleOrDefault();

            if (customer != null)
                return customer;

            //TODO add specific type of exception
            throw new Exception(string.Format("Customer with Id {0} not found", customerId));
        }
    }
}
