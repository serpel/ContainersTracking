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
    [AccessAuthorizeAttribute(Roles = "Admin, Manager")]
    public class RegionsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult GetRegions()
        {
            var regions = db.Regions.Select(s => new { s.Name, s.RegionId });

            return Json(regions.ToList(), JsonRequestBehavior.AllowGet);
        }

        // GET: Regions
        public ActionResult Index()
        {
            return View();
        }

        // GET: Regions/Create
        public ActionResult Create()
        {
            return PartialView("Create");
        }

        // POST: Regions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RegionId,Name")] Region region)
        {
            if (ModelState.IsValid)
            {
                db.Regions.Add(region);
                db.SaveChanges();

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return PartialView("Create", region);
        }

        // GET: Regions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Region region = db.Regions.Find(id);
            if (region == null)
            {
                return HttpNotFound();
            }
            return PartialView("Edit", region);
        }

        // POST: Regions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RegionId,Name")] Region region)
        {
            if (ModelState.IsValid)
            {
                db.Entry(region).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return PartialView("Edit", region);
        }

        // GET: Regions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Region region = db.Regions.Find(id);
            if (region == null)
            {
                return HttpNotFound();
            }
            return PartialView("Delete", region);
        }

        // POST: Regions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Region region = db.Regions.Find(id);
            db.Regions.Remove(region);
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
