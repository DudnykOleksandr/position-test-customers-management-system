using Data.Models;
using Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;

namespace UnitTests
{
    public class CustomersControllerTests
    {
        public CustomersControllerTests()
        {
        }

        [Fact]
        public void GetAll_NoClaim_ReturnStatus500()
        {
            // Arrange
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var controller = new CustomersController(customerRepositoryMock.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new List<ClaimsIdentity>());

            // Act
            var result = controller.GetAll();

            // Assert
            Assert.IsType<JsonResult>(result);
            Assert.True(result.StatusCode == StatusCodes.Status500InternalServerError);
            customerRepositoryMock.Verify(foo => foo.GetAll(string.Empty), Times.Never());
        }
        [Fact]
        public void GetAll_AdminClaim_RepositoryIsCalled()
        {
            // Arrange
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var controller = new CustomersController(customerRepositoryMock.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(Constants.AdminClaimTypeName, string.Empty));
            controller.ControllerContext.HttpContext.User =  new ClaimsPrincipal(claimsIdentity);

            // Act
            var result = controller.GetAll();

            // Assert
            var jsonResult=Assert.IsType<JsonResult>(result);
            Assert.NotNull(jsonResult.Value);
            customerRepositoryMock.Verify(foo => foo.GetAll(string.Empty), Times.Once());
        }
    }
}
