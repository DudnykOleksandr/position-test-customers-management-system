using Data.Repositories;
using Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Linq;
using Bussiness;
using Microsoft.AspNetCore.Authorization;
using System;
using log4net;

namespace Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAccountManager _accountManager;
        private static readonly ILog log = LogManager.GetLogger(typeof(AccountController));

        public AccountController(ICustomerRepository customerRepository, IAccountManager accountManager)
        {
            _customerRepository = customerRepository;
            _accountManager = accountManager;
        }

        [HttpGet]
        public JsonResult IsUserNameUnique(string userName)
        {
            try
            {
                var existingUserNames = _customerRepository.GetAllUserNames();
                if (existingUserNames.Contains(userName))
                    return Json(true);

                return Json(false);
            }
            catch (Exception ex)
            {
                log.Error(ex);

                var result = Json(null);
                result.StatusCode = StatusCodes.Status500InternalServerError;

                return result;
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string userName, string password)
        {
            ViewBag.ShowErrMsg = false;

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                try
                {
                    var user = _accountManager.VerifyUserPassword(userName, password);
                    if (user != null)
                    {
                        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                        identity.AddClaim(new Claim(ClaimTypes.Name, userName));

                        if (user.Role == UserRole.AdminUser)
                            identity.AddClaim(new Claim(Constants.AdminClaimTypeName, string.Empty));
                        else
                            identity.AddClaim(new Claim(Constants.CustomerIdClaimTypeName, user.CustomerId.ToString()));

                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                        return RedirectToAction("Index", "Customers");
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    return View("Error");
                }
            }

            ViewBag.ShowErrMsg = true;

            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }

        //[HttpGet]
        //public EmptyResult AddDefaultAdminUser()
        //{
        //    var user = new User();
        //    user.UserId = Guid.NewGuid();
        //    user.UserName = "Admin";
        //    user.Role = UserRole.AdminUser;
        //    user.PasswordHashSalt = AccountManager.GenerateRandomSalt();
        //    user.PasswordHash = AccountManager.Hash(user.PasswordHashSalt, "P@ssw0rd");
        //    user.Email = "admin@admin";
        //    user.FirstName = "admin";
        //    user.Phone = "empty";
        //    _customerRepository.CreateUser(user);

        //    return new EmptyResult();
        //}
    }
}