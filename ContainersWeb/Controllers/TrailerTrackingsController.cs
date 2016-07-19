using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContainersWeb.Models;
using ContainersWeb.BLL;

namespace ContainersWeb.Controllers
{
    public class TrailerTrackingsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult GetTrailerTrackings()
        {
            var containers = db.TrailerTracking
                .ToList()
                .Select(s => new {
                    s.TrailerTrackingId,
                    Type = s.Type == 0 ? Resources.Resources.Out : Resources.Resources.In,
                    DocStatus = s.DocStatus == 0 ? Resources.Resources.Pending : Resources.Resources.Ready,
                    ContainerStatus = s.ContainerStatus == 0 ? Resources.Resources.Empty : Resources.Resources.Full,
                    Date = s.InsertedAt.ToString("yyyy-MM-dd hh:mm"),
                    s.TrailerNumber,
                    s.Notes                
                });

            return Json(containers, JsonRequestBehavior.AllowGet);
        }

        // GET: TrailerTrackings
        public ActionResult Index()
        {
             return View();
        }

        // GET: TrailerTrackings/Create
        public ActionResult Create()
        {
            var trailer = new TrailerTracking();
            trailer.InsertedAt = DateTime.Now;
            trailer.InsertedBy = User.Identity.Name;

            return PartialView("Create", trailer);
        }

        // POST: TrailerTrackings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TrailerTrackingId,Type,CompanyOriginId,CompanyDestinationId,ContainerStatus,DocStatus,TrailerNumber,ContainerLicensePlate,ContainerLabel,DocNumber,Notes,DriverId,SecuritySupervisorId,InsertedAt,UpdatedAt,InsertedBy,UpdatedBy")] TrailerTracking trailerTracking)
        {
            trailerTracking.UpdatedAt = DateTime.Now;
            trailerTracking.DocStatus = DocStatus.Pendiente;

            TrailerTrackingHelper validator = new TrailerTrackingHelper(db);

            if (ModelState.IsValid)
            {
                if (validator.ValidateOnCreate(trailerTracking))
                {
                    db.TrailerTracking.Add(trailerTracking);
                    db.SaveChanges();

                    MyLogger.GetInstance.Info(Resources.Resources.CreatedText + " TrailerTrackingId: " + trailerTracking.TrailerTrackingId + " TrailerNumber: " + trailerTracking.TrailerNumber);

                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ModelState.AddModelError("TrailerNumber", validator.Message);
                }
            }

            return PartialView("Create", trailerTracking);
        }

        // GET: TrailerTrackings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrailerTracking trailerTracking = db.TrailerTracking.Find(id);
            if (trailerTracking == null)
            {
                return HttpNotFound();
            }

            trailerTracking.UpdatedAt = DateTime.Now;
            trailerTracking.UpdatedBy = User.Identity.Name;

            ViewBag.CompanyDestinationId = new SelectList(db.Companies, "CompanyId", "Name", trailerTracking.CompanyDestinationId);
            ViewBag.CompanyOriginId = new SelectList(db.Companies, "CompanyId", "Name", trailerTracking.CompanyOriginId);
            ViewBag.DriverId = new SelectList(db.Drivers, "DriverId", "Name", trailerTracking.DriverId);
            ViewBag.SecuritySupervisorId = new SelectList(db.SecuritySupervisors, "SecuritySupervisorId", "Name", trailerTracking.SecuritySupervisorId);
            return PartialView("Edit", trailerTracking);
        }

        // POST: TrailerTrackings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TrailerTrackingId,Type,CompanyOriginId,CompanyDestinationId,ContainerStatus,DocStatus,TrailerNumber,ContainerLicensePlate,ContainerLabel,DocNumber,Notes,DriverId,SecuritySupervisorId,InsertedAt,UpdatedAt,InsertedBy,UpdatedBy")] TrailerTracking trailerTracking)
        {
            trailerTracking.UpdatedAt = DateTime.Now;

            TrailerTrackingHelper validator = new TrailerTrackingHelper(db);

            if (ModelState.IsValid)
            {
                if (validator.ValidateOnEdit(trailerTracking))
                {
                    db.Entry(trailerTracking).State = EntityState.Modified;
                    db.SaveChanges();

                    MyLogger.GetInstance.Info(Resources.Resources.EditText + " TrailerTrackingId: " + trailerTracking.TrailerTrackingId + " ContainerNumber: " + trailerTracking.TrailerNumber);

                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            ViewBag.CompanyDestinationId = new SelectList(db.Companies, "CompanyId", "Name", trailerTracking.CompanyDestinationId);
            ViewBag.CompanyOriginId = new SelectList(db.Companies, "CompanyId", "Name", trailerTracking.CompanyOriginId);
            ViewBag.DriverId = new SelectList(db.Drivers, "DriverId", "Name", trailerTracking.DriverId);
            ViewBag.SecuritySupervisorId = new SelectList(db.SecuritySupervisors, "SecuritySupervisorId", "Name", trailerTracking.SecuritySupervisorId);
            return PartialView("Edit", trailerTracking);
        }

        // GET: TrailerTrackings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrailerTracking trailerTracking = db.TrailerTracking.Find(id);
            if (trailerTracking == null)
            {
                return HttpNotFound();
            }
            return PartialView("Delete", trailerTracking);
        }

        // POST: TrailerTrackings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TrailerTracking trailerTracking = db.TrailerTracking.Find(id);
            db.TrailerTracking.Remove(trailerTracking);
            db.SaveChanges();

            MyLogger.GetInstance.Info(Resources.Resources.DeletedText + " ContainerTrackingId: " + trailerTracking.TrailerTrackingId + " ContainerNumber: " + trailerTracking.TrailerNumber);

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
