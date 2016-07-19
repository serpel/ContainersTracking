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
    public class TruckTrackingsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TruckTrackings
        public ActionResult Index()
        {
            var truckTracking = db.TruckTracking.Include(t => t.CompanyDestination).Include(t => t.CompanyOrigin).Include(t => t.Driver).Include(t => t.SecuritySupervisor);
            return View(truckTracking.ToList());
        }

        // GET: TruckTrackings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TruckTracking truckTracking = db.TruckTracking.Find(id);
            if (truckTracking == null)
            {
                return HttpNotFound();
            }
            return View(truckTracking);
        }

        // GET: TruckTrackings/Create
        public ActionResult Create()
        {
            ViewBag.CompanyDestinationId = new SelectList(db.Companies, "CompanyId", "Name");
            ViewBag.CompanyOriginId = new SelectList(db.Companies, "CompanyId", "Name");
            ViewBag.DriverId = new SelectList(db.Drivers, "DriverId", "Name");
            ViewBag.SecuritySupervisorId = new SelectList(db.SecuritySupervisors, "SecuritySupervisorId", "Name");
            return View();
        }

        // POST: TruckTrackings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TruckTrackingTrackingId,Type,CompanyOriginId,CompanyDestinationId,ContainerStatus,DocStatus,ContainerNumber,ContainerLicensePlate,ContainerLabel,DocNumber,CorrelAduana,DriverId,SecuritySupervisorId,InsertedAt,UpdatedAt,InsertedBy,UpdatedBy")] TruckTracking truckTracking)
        {
            if (ModelState.IsValid)
            {
                db.TruckTracking.Add(truckTracking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyDestinationId = new SelectList(db.Companies, "CompanyId", "Name", truckTracking.CompanyDestinationId);
            ViewBag.CompanyOriginId = new SelectList(db.Companies, "CompanyId", "Name", truckTracking.CompanyOriginId);
            ViewBag.DriverId = new SelectList(db.Drivers, "DriverId", "Name", truckTracking.DriverId);
            ViewBag.SecuritySupervisorId = new SelectList(db.SecuritySupervisors, "SecuritySupervisorId", "Name", truckTracking.SecuritySupervisorId);
            return View(truckTracking);
        }

        // GET: TruckTrackings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TruckTracking truckTracking = db.TruckTracking.Find(id);
            if (truckTracking == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyDestinationId = new SelectList(db.Companies, "CompanyId", "Name", truckTracking.CompanyDestinationId);
            ViewBag.CompanyOriginId = new SelectList(db.Companies, "CompanyId", "Name", truckTracking.CompanyOriginId);
            ViewBag.DriverId = new SelectList(db.Drivers, "DriverId", "Name", truckTracking.DriverId);
            ViewBag.SecuritySupervisorId = new SelectList(db.SecuritySupervisors, "SecuritySupervisorId", "Name", truckTracking.SecuritySupervisorId);
            return View(truckTracking);
        }

        // POST: TruckTrackings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TruckTrackingTrackingId,Type,CompanyOriginId,CompanyDestinationId,ContainerStatus,DocStatus,ContainerNumber,ContainerLicensePlate,ContainerLabel,DocNumber,CorrelAduana,DriverId,SecuritySupervisorId,InsertedAt,UpdatedAt,InsertedBy,UpdatedBy")] TruckTracking truckTracking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(truckTracking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyDestinationId = new SelectList(db.Companies, "CompanyId", "Name", truckTracking.CompanyDestinationId);
            ViewBag.CompanyOriginId = new SelectList(db.Companies, "CompanyId", "Name", truckTracking.CompanyOriginId);
            ViewBag.DriverId = new SelectList(db.Drivers, "DriverId", "Name", truckTracking.DriverId);
            ViewBag.SecuritySupervisorId = new SelectList(db.SecuritySupervisors, "SecuritySupervisorId", "Name", truckTracking.SecuritySupervisorId);
            return View(truckTracking);
        }

        // GET: TruckTrackings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TruckTracking truckTracking = db.TruckTracking.Find(id);
            if (truckTracking == null)
            {
                return HttpNotFound();
            }
            return View(truckTracking);
        }

        // POST: TruckTrackings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TruckTracking truckTracking = db.TruckTracking.Find(id);
            db.TruckTracking.Remove(truckTracking);
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
