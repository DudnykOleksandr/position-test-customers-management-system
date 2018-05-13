using Data.Models;
using Data.Repositories;
using log4net;
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
        private static readonly ILog log = LogManager.GetLogger(typeof(CustomersController));

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public ViewResult Index()
        {
            var adminClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == Constants.AdminClaimTypeName);
            ViewBag.IsAdmin = adminClaim != null ? true : false;

            return View();
        }

        public JsonResult GetAll()
        {
            try
            {
                var customerId = string.Empty;
                var customerIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == Constants.CustomerIdClaimTypeName);
                if (customerIdClaim != null)
                    customerId = customerIdClaim.Value;
                else
                {
                    var adminClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == Constants.AdminClaimTypeName);
                    if (adminClaim == null)
                        RedirectToAction("Login", "Account");
                }
                var allCustomerDtos = _customerRepository.GetAll(customerId).ToDataModels().ToList();
                return Json(allCustomerDtos);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                var result = Json(null);
                result.StatusCode = StatusCodes.Status500InternalServerError;

                return result;
            }
        }

        [HttpPost]
        [Authorize(Policy = Constants.AdminPolicyName)]
        public IActionResult Save([FromBody]CustomerDto customerDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _customerRepository.Save(customerDto.ToDataModel());
                    var allCustomerDtos = _customerRepository.GetAll().ToDataModels().ToList();
                    return Ok(allCustomerDtos);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [Authorize(Policy = Constants.AdminPolicyName)]
        public IActionResult Delete([FromBody]CustomerDto customerDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _customerRepository.Delete(customerDto.ToDataModel());
                    var allCustomerDtos = _customerRepository.GetAll().ToDataModels().ToList();
                    return Ok(allCustomerDtos);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
            return BadRequest();
        }
    }
}
