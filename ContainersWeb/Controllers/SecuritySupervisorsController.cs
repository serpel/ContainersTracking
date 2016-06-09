using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContainersWeb.Models;
using ContainersWeb.DAL.Security;

namespace ContainersWeb.Controllers
{
    [AccessAuthorizeAttribute(Roles = "Admin, Manager, User")]
    public class SecuritySupervisorsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult GetSecuritySupervisors()
        {
            var securitySupervisors = db.SecuritySupervisors.Select(s => new { s.SecuritySupervisorId, s.Name, s.CardId, s.IsActive });

            return Json(securitySupervisors.ToList(), JsonRequestBehavior.AllowGet);
        }


        // GET: SecuritySupervisors
        public ActionResult Index()
        {
            return View();
        }

        // GET: SecuritySupervisors/Create
        public ActionResult Create()
        {
            return PartialView("Create");
        }

        // POST: SecuritySupervisors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SecuritySupervisorId,Name,CardId,IsActive")] SecuritySupervisor securitySupervisor)
        {
            if (ModelState.IsValid)
            {
                db.SecuritySupervisors.Add(securitySupervisor);
                db.SaveChanges();

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return PartialView("Create", securitySupervisor);
        }

        // GET: SecuritySupervisors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SecuritySupervisor securitySupervisor = db.SecuritySupervisors.Find(id);
            if (securitySupervisor == null)
            {
                return HttpNotFound();
            }
            return PartialView("Edit", securitySupervisor);
        }

        // POST: SecuritySupervisors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SecuritySupervisorId,Name,CardId,IsActive")] SecuritySupervisor securitySupervisor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(securitySupervisor).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return PartialView("Edit", securitySupervisor);
        }

        // GET: SecuritySupervisors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SecuritySupervisor securitySupervisor = db.SecuritySupervisors.Find(id);
            if (securitySupervisor == null)
            {
                return HttpNotFound();
            }
            return PartialView("Delete", securitySupervisor);
        }

        // POST: SecuritySupervisors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SecuritySupervisor securitySupervisor = db.SecuritySupervisors.Find(id);
            db.SecuritySupervisors.Remove(securitySupervisor);
            db.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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
