using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContainersWeb.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        // GET: AccessDenied
        public ActionResult AccessDenied()
        {
            ViewBag.ErrorCode = 401;
            ViewBag.Description = "Access Denied";

            return View();
        }

        // GET: TODO: Display error on fancy page Error
        public ActionResult Error()
        {
            return View();
        }
    }
}