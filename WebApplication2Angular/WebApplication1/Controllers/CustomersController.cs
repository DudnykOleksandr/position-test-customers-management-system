using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1
{
    public class CustomersController : Controller
    {
        private readonly CustomerDBContext _context;

        public CustomersController(CustomerDBContext context)
        {
            _context = context;
        }

        //[Authorize]
        public ViewResult Index()
        {
            return View();
        }

        //[Authorize]
        public JsonResult GetAllCustomers()
        {
            return Json(_context.Customer.Include(c => c.Contacts).ToList());
        }

        //[Authorize]
        [HttpPost]
        public IActionResult CreateFromJson([FromBody]Customer customer)
        {
            if (ModelState.IsValid)
            {
                Customer customerToSave = null;
                bool isNewCustomer = true;
                if (customer.CustomerId == Guid.Empty)
                {
                    customer.CustomerId = Guid.NewGuid();
                    _context.Customer.Add(customer);
                    customerToSave = new Customer();
                    customerToSave.Name = customer.Name;
                }
                else
                {
                    isNewCustomer = false;
                    customerToSave = _context.Customer.Where(c => c.CustomerId == customer.CustomerId).Include(c => c.Contacts).Single();
                    //customerToSave.Name = customer.Name;
                    _context.Entry(customerToSave).CurrentValues.SetValues(customer);
                }

                foreach (var contact in customer.Contacts)
                {
                    if (contact.ContactId == Guid.Empty)
                    {
                        contact.ContactId = Guid.NewGuid();
                        customerToSave.Contacts.Add(contact);
                        _context.Contact.Add(contact);
                    }
                    else
                    {
                        var existingContact = _context.Contact.Where(c => c.ContactId == contact.ContactId).Single();
                        //existingContact.Name = contact.Name;
                        _context.Entry(existingContact).CurrentValues.SetValues(contact);
                    }
                }
                if (!isNewCustomer)
                {
                    var contactIds = customer.Contacts.Select(c => c.ContactId);
                    var contactsToRemove = customerToSave.Contacts.Where(c => !contactIds.Contains(c.ContactId));
                    foreach (var contactToRemove in contactsToRemove)
                        _context.Contact.Remove(contactToRemove);
                }

                _context.SaveChanges();
            }
            return Ok(_context.Customer.ToList());
        }
    }
}
