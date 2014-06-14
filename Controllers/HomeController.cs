using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XiyouNews.Models;

namespace XiyouNews.Controllers
{
    public class HomeController : ApiController
    {
        [HttpGet]
        [HttpPost]
        public UniResult Index()
        {
            return new UniResult
            {
                Result = false,
                Detail = "REQUEST_ERROR"
            };
        }
    }
}
