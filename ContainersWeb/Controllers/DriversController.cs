﻿using System;
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
    public class DriversController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult GetDrivers()
        {
            var drivers = db.Drivers.Select(s => new { s.DriverId, s.Name, s.CardId, s.IsActive });

            return Json(drivers.ToList(), JsonRequestBehavior.AllowGet);
        }

        // GET: Drivers
        public ActionResult Index()
        {
            return View();
        }

        // GET: Drivers/Create
        public ActionResult Create()
        {
            var driver = new Driver() { IsActive = true };

            return PartialView("Create", driver);
        }

        // POST: Drivers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DriverId,Name,CardId,IsActive")] Driver driver)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Drivers.Add(driver);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("CardId", e);
                    return PartialView("Create", driver);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return PartialView("Create", driver);
        }

        // GET: Drivers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Driver driver = db.Drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            return PartialView("Edit", driver);
        }

        // POST: Drivers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DriverId,Name,CardId,IsActive")] Driver driver)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(driver).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("CardId", e);
                    return PartialView("Edit", driver);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return PartialView("Edit", driver);
        }

        // GET: Drivers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Driver driver = db.Drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            return PartialView("Delete", driver);
        }

        // POST: Drivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Driver driver = db.Drivers.Find(id);
            db.Drivers.Remove(driver);
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
