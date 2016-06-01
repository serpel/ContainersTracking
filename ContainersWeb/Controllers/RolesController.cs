using ContainersWeb.BLL;
using ContainersWeb.DAL.Security;
using ContainersWeb.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContainersWeb.Controllers
{

    [AccessAuthorizeAttribute(Roles = "Admin")]
    public class RolesController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Role
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetRoles()
        {
            var result = db.Roles
                .Select(s => new { s.Id, s.Name });

            return Json(result.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            var role = new IdentityRole();
            return PartialView("Create", role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                db.Roles.Add(role);
                db.SaveChanges();

                MyLogger.GetInstance.Info("Role was created successfull: " + role.Name);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return PartialView("Create", role);
        }

        public ActionResult Edit(int id)
        {
            var role = db.Roles.Find(id);
            return PartialView("Edit", role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                db.Entry(role).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                MyLogger.GetInstance.Info("Role was edited successfull: " + role.Name);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return PartialView("Edit", role);
        }
    }
}