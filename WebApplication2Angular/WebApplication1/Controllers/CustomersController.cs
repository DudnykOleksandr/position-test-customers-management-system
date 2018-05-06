using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public ViewResult Index()
        {
            return View();
        }

        public JsonResult GetAllCustomers()
        {
            return Json(_context.Customer.ToList());
        }

        [HttpPost]
        public IActionResult CreateFromJson([FromBody]Customer customer)
        {
            if (customer.CustomerId == Guid.Empty) {
                customer.CustomerId = Guid.NewGuid();
                _context.Entry(customer).State = EntityState.Added;
            }
            else
                _context.Entry(customer).State = EntityState.Modified;

            foreach (var contact in customer.Contacts)
            {
                if (contact.ContactId == Guid.Empty)
                {
                    contact.ContactId = Guid.NewGuid();
                    _context.Entry(contact).State = EntityState.Added;
                }
                else
                    _context.Entry(contact).State = EntityState.Modified;
            }

            if (ModelState.IsValid)
            {
                _context.Add(customer);
                _context.SaveChanges();
            }
            return Ok(_context.Customer.ToList());
        }
    }
}
