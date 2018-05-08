using Microsoft.AspNetCore.Mvc;
using System;

namespace Presentation.Controllers
{
    public class CommonController : Controller
    {
        [HttpGet]
        public JsonResult GetGuid()
        {
            return  Json(Guid.NewGuid().ToString());
        }
    }
}