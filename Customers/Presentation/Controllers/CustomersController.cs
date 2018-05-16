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
        private static readonly ILog log = LogManager.GetLogger(typeof(CustomersController));
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public ViewResult Index()
        {
            var adminClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == Constants.AdminClaimTypeName);
            ViewBag.IsAdmin = adminClaim != null ? true : false;

            return View();
        }

        // / <summary>
        // / Returns all customers or specific one if user is not admin
        // / </summary>
        // / <returns></returns>
        [HttpGet]
        public JsonResult GetAll()
        {
            try
            {
                // checking regular user claim for customer id
                var customerId = string.Empty;
                var customerIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == Constants.CustomerIdClaimTypeName);
                if (customerIdClaim != null)
                    customerId = customerIdClaim.Value;
                else
                {
                    // if regular user claim is not found trying to check if it is admin user
                    var adminClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == Constants.AdminClaimTypeName);
                    if (adminClaim == null)
                        // if admin claim is not found redirect to login
                        throw new Exception("Current user claim is not found");
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

        // / <summary>
        // / Saves complex cutomer object. Available only for admin user
        // / </summary>
        // / <param name="customerDto"></param>
        // / <returns></returns>
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

        // / <summary>
        // / Deletes complex customer object. Available only for admin user.
        // / </summary>
        // / <param name="customerDto"></param>
        // / <returns></returns>
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
