using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContainersWeb.Models;

namespace ContainersWeb.Controllers
{
    public class ContainerTrackingsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ContainerTrackings
        public ActionResult Index()
        {
            var containerTracking = db.ContainerTracking.Include(c => c.CompanyDestination).Include(c => c.CompanyOrigin).Include(c => c.Driver).Include(c => c.SecuritySupervisor);
            return View(containerTracking.ToList());
        }

        // GET: ContainerTrackings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContainerTracking containerTracking = db.ContainerTracking.Find(id);
            if (containerTracking == null)
            {
                return HttpNotFound();
            }
            return View(containerTracking);
        }

        // GET: ContainerTrackings/Create
        public ActionResult Create()
        {
            ViewBag.CompanyDestinationId = new SelectList(db.Companies, "CompanyId", "Name");
            ViewBag.CompanyOriginId = new SelectList(db.Companies, "CompanyId", "Name");
            ViewBag.DriverId = new SelectList(db.Drivers, "DriverId", "Name");
            ViewBag.SecuritySupervisorId = new SelectList(db.SecuritySupervisors, "SecuritySupervisorId", "Name");
            return View();
        }

        // POST: ContainerTrackings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TrackingId,Type,CompanyOriginId,CompanyDestinationId,DocStatus,ContainerNumber,ContainerLicensePlate,ContainerLabel,ChasisNumber,DuaNumber,DriverId,SecuritySupervisorId,InsertedAt,UpdatedAt")] ContainerTracking containerTracking)
        {
            if (ModelState.IsValid)
            {
                db.ContainerTracking.Add(containerTracking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyDestinationId = new SelectList(db.Companies, "CompanyId", "Name", containerTracking.CompanyDestinationId);
            ViewBag.CompanyOriginId = new SelectList(db.Companies, "CompanyId", "Name", containerTracking.CompanyOriginId);
            ViewBag.DriverId = new SelectList(db.Drivers, "DriverId", "Name", containerTracking.DriverId);
            ViewBag.SecuritySupervisorId = new SelectList(db.SecuritySupervisors, "SecuritySupervisorId", "Name", containerTracking.SecuritySupervisorId);
            return View(containerTracking);
        }

        // GET: ContainerTrackings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContainerTracking containerTracking = db.ContainerTracking.Find(id);
            if (containerTracking == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyDestinationId = new SelectList(db.Companies, "CompanyId", "Name", containerTracking.CompanyDestinationId);
            ViewBag.CompanyOriginId = new SelectList(db.Companies, "CompanyId", "Name", containerTracking.CompanyOriginId);
            ViewBag.DriverId = new SelectList(db.Drivers, "DriverId", "Name", containerTracking.DriverId);
            ViewBag.SecuritySupervisorId = new SelectList(db.SecuritySupervisors, "SecuritySupervisorId", "Name", containerTracking.SecuritySupervisorId);
            return View(containerTracking);
        }

        // POST: ContainerTrackings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TrackingId,Type,CompanyOriginId,CompanyDestinationId,DocStatus,ContainerNumber,ContainerLicensePlate,ContainerLabel,ChasisNumber,DuaNumber,DriverId,SecuritySupervisorId,InsertedAt,UpdatedAt")] ContainerTracking containerTracking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(containerTracking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyDestinationId = new SelectList(db.Companies, "CompanyId", "Name", containerTracking.CompanyDestinationId);
            ViewBag.CompanyOriginId = new SelectList(db.Companies, "CompanyId", "Name", containerTracking.CompanyOriginId);
            ViewBag.DriverId = new SelectList(db.Drivers, "DriverId", "Name", containerTracking.DriverId);
            ViewBag.SecuritySupervisorId = new SelectList(db.SecuritySupervisors, "SecuritySupervisorId", "Name", containerTracking.SecuritySupervisorId);
            return View(containerTracking);
        }

        // GET: ContainerTrackings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContainerTracking containerTracking = db.ContainerTracking.Find(id);
            if (containerTracking == null)
            {
                return HttpNotFound();
            }
            return View(containerTracking);
        }

        // POST: ContainerTrackings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContainerTracking containerTracking = db.ContainerTracking.Find(id);
            db.ContainerTracking.Remove(containerTracking);
            db.SaveChanges();
            return RedirectToAction("Index");
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
