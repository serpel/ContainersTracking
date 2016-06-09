using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContainersWeb.Models;
using System.Web.Security;
using ContainersWeb.DAL.Security;

namespace ContainersWeb.Controllers
{
    [AccessAuthorizeAttribute(Roles = "Admin")]
    public class LogEntriesController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult GetLogEntries()
        {
            var entries = db.LogEntries
                .Where(w => w.Level == "Info")
                .ToList()
                .Select(s => new { Date = s.Date.ToString("yyyy-MM-dd hh:mm"), s.Message, s.Username, s.Url });

            return Json(entries.ToList(), JsonRequestBehavior.AllowGet);
        }

        // GET: LogEntries
        public ActionResult Index()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
