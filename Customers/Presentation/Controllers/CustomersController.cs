using Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Dtos;
using System;
using System.Linq;

namespace Presentation.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public ViewResult Index()
        {
            var adminClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "IsAdmin");
            if (adminClaim != null)
                ViewBag.IsAdmin = true;

            return View();
        }

        public JsonResult GetAll()
        {
            var customerId = string.Empty;
            var customerIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "CustomerId");
            if (customerIdClaim != null)
            {
                customerId = customerIdClaim.Value;
            }
            var allCustomerDtos = _customerRepository.GetAll(customerId).ToDataModels().ToList();
            return Json(allCustomerDtos);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Save([FromBody]CustomerDto customerDto)
        {
            if (ModelState.IsValid)
            {
                _customerRepository.Save(customerDto.ToDataModel());
                var allCustomerDtos = _customerRepository.GetAll().ToDataModels().ToList();
                return Ok(allCustomerDtos);
            }
            return BadRequest();
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Delete([FromBody]CustomerDto customerDto)
        {
            if (ModelState.IsValid)
            {
                _customerRepository.Delete(customerDto.ToDataModel());
                var allCustomerDtos = _customerRepository.GetAll().ToDataModels().ToList();
                return Ok(allCustomerDtos);
            }
            return BadRequest();
        }
    }
}
