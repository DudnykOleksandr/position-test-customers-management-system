using Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Presentation.Dtos;
using System.Linq;

namespace Presentation.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        //[Authorize]
        public ViewResult Index()
        {
            return View();
        }

        //[Authorize]
        public JsonResult GetAllCustomers()
        {
            var allCustomerDtos = _customerRepository.GetAllCustomers().ToDataModels().ToList();
            return Json(allCustomerDtos);
        }

        //[Authorize]
        [HttpPost]
        public IActionResult CreateFromJson([FromBody]CustomerDto customerDto)
        {
            if (ModelState.IsValid)
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
                _customerRepository.SaveCustomer(customerDto.ToDataModel());
            }
            var allCustomerDtos = _customerRepository.GetAllCustomers().ToDataModels().ToList();

            return Ok(allCustomerDtos);
        }
    }
}
