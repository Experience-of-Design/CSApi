using System.IdentityModel;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using OAuth2DotNet.Client;
using OAuth2DotNet.Common;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Web;

namespace APIClient.Controllers
{
    public class CallApiController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}