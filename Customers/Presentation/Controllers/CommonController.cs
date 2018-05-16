using Microsoft.AspNetCore.Mvc;
using System;

namespace Presentation.Controllers
{
    public class CommonController : Controller
    {
        // / <summary>
        // / Returns stringified value of new guid
        // / </summary>
        // / <param name="numberOfGuidToGet"></param>
        // / <returns></returns>
        [HttpGet]
        public JsonResult GetGuid(int numberOfGuidToGet)
        {
            var guids = new Guid[numberOfGuidToGet];
            for (int i = 0; i < numberOfGuidToGet; i++)
            {
                guids[i] = Guid.NewGuid();
            }
            return  Json(guids);
        }
    }
}