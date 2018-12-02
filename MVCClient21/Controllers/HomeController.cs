/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MVCClient21.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration mConfig;
        private ILogger mLogger;
        public HomeController(IConfiguration config,ILogger<HomeController> logger)
        {
            mConfig = config;
            mLogger = logger;
        }

        [HttpGet("~/")]
        public ActionResult Index()
        {
            ViewBag.Msg = mConfig.GetSection("MyConfiguration").GetValue<string>("Msg");
            mLogger.LogInformation("you visit:[{0}]",this.Request.GetEncodedPathAndQuery());
            return View();
        }
    }
}
