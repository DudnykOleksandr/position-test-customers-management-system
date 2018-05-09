using Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Presentation.Dtos;
using System;
using System.Linq;

namespace Presentation.Controllers
{
    //[Authorize]
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
     
        public ViewResult Index()
        {
            return View();
        }

        public JsonResult GetAll()
        {
            var allCustomerDtos = _customerRepository.GetAll().ToDataModels().ToList();
            return Json(allCustomerDtos);
        }

        [HttpPost]
        public IActionResult Save([FromBody]CustomerDto customerDto)
        {
            if (ModelState.IsValid)
            {
                _customerRepository.Save(customerDto.ToDataModel());
            }
            var allCustomerDtos = _customerRepository.GetAll().ToDataModels().ToList();

            return Ok(allCustomerDtos);
        }

        [HttpGet]
        public IActionResult Delete(string customerId)
        {
            var customerGuid = Guid.Parse(customerId);
            _customerRepository.Delete(customerGuid);
            var allCustomerDtos = _customerRepository.GetAll().ToDataModels().ToList();

            return Ok(allCustomerDtos);
        }
    }
}
