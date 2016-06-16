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
using ContainersWeb.DAL.Security;

namespace ContainersWeb.Controllers
{
    [AccessAuthorizeAttribute(Roles = "Admin, Manager, User")]
    public class ContainerTrackingsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult GetContainerTrackings()
        {
            var containers = db.ContainerTracking
                .ToList()
                .Select(s => new { s.ContainerTrackingId, Type = s.Type == 0 ? Resources.Resources.Out: Resources.Resources.In,
                    DocStatus = s.DocStatus == 0 ? Resources.Resources.Pending : Resources.Resources.Ready,
                    ContainerStatus = s.ContainerStatus == 0 ? Resources.Resources.Empty : Resources.Resources.Full,
                    Date = s.InsertedAt.ToString("yyyy-MM-dd hh:mm"), s.ChasisNumber, s.ContainerNumber });

            return Json(containers, JsonRequestBehavior.AllowGet);
        }

        // GET: ContainerTrackings
        public ActionResult Index()
        {           
            return View();
        }

        // GET: ContainerTrackings/Create
        public ActionResult Create()
        {
            var container = new ContainerTracking();
            container.InsertedAt = DateTime.Now;
            container.InsertedBy = User.Identity.Name;
                    
            return PartialView("Create", container);
        }

        // POST: ContainerTrackings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContainerTrackingId,Type,CompanyOriginId,CompanyDestinationId,DocStatus,ContainerNumber,ContainerStatus,ContainerLicensePlate,ContainerLabel,ChasisNumber,DocNumber,CorrelAduana,DriverId,SecuritySupervisorId,InsertedAt,InsertedBy,UpdatedAt,UpdatedBy")] ContainerTracking containerTracking)
        {
            containerTracking.UpdatedAt = DateTime.Now;
            containerTracking.DocStatus = DocStatus.Pending;

            ContainerTrackingHelper validator = new ContainerTrackingHelper(db);

            if (ModelState.IsValid)
            {
                if (validator.ValidateOnCreate(containerTracking))
                {
                    db.ContainerTracking.Add(containerTracking);
                    db.SaveChanges();

                    MyLogger.GetInstance.Info(Resources.Resources.CreatedText + " ContainerTrackingId: " + containerTracking.ContainerTrackingId + " ContainerNumber: " + containerTracking.ContainerNumber);

                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ModelState.AddModelError("ContainerNumber", validator.Message);
                }
            }

            return PartialView("Create", containerTracking);
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
            containerTracking.UpdatedAt = DateTime.Now;
            containerTracking.UpdatedBy = User.Identity.Name;

            ViewBag.CompanyDestinationId = new SelectList(db.Companies.Where(w => w.IsActive == true), "CompanyId", "Name", containerTracking.CompanyDestinationId);
            ViewBag.CompanyOriginId = new SelectList(db.Companies.Where(w => w.IsActive == true), "CompanyId", "Name", containerTracking.CompanyOriginId);
            ViewBag.DriverId = new SelectList(db.Drivers, "DriverId", "Name", containerTracking.DriverId);
            ViewBag.SecuritySupervisorId = new SelectList(db.SecuritySupervisors.Where(w => w.IsActive == true), "SecuritySupervisorId", "Name", containerTracking.SecuritySupervisorId);
            return PartialView("Edit", containerTracking);
        }

        // POST: ContainerTrackings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContainerTrackingId,Type,CompanyOriginId,CompanyDestinationId,DocStatus,ContainerNumber,ContainerStatus,ContainerLicensePlate,ContainerLabel,ChasisNumber,DocNumber,CorrelAduana,DriverId,SecuritySupervisorId,InsertedAt,InsertedBy,UpdatedAt,UpdatedBy")] ContainerTracking containerTracking)
        {
            containerTracking.UpdatedAt = DateTime.Now;

            ContainerTrackingHelper validator = new ContainerTrackingHelper(db);

            if (ModelState.IsValid)
            {
                if (validator.ValidateOnEdit(containerTracking))
                {
                    db.Entry(containerTracking).State = EntityState.Modified;
                    db.SaveChanges();

                    MyLogger.GetInstance.Info(Resources.Resources.EditText + " ContainerTrackingId: "+containerTracking.ContainerTrackingId+ " ContainerNumber: "+containerTracking.ContainerNumber);                    

                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            ViewBag.CompanyDestinationId = new SelectList(db.Companies.Where(w => w.IsActive == true), "CompanyId", "Name", containerTracking.CompanyDestinationId);
            ViewBag.CompanyOriginId = new SelectList(db.Companies.Where(w => w.IsActive == true), "CompanyId", "Name", containerTracking.CompanyOriginId);
            ViewBag.DriverId = new SelectList(db.Drivers, "DriverId", "Name", containerTracking.DriverId);
            ViewBag.SecuritySupervisorId = new SelectList(db.SecuritySupervisors.Where(w => w.IsActive == true), "SecuritySupervisorId", "Name", containerTracking.SecuritySupervisorId);
            return PartialView("Edit", containerTracking);
        }

        [AccessAuthorizeAttribute(Roles = "Admin")]
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
            return PartialView("Delete", containerTracking);
        }

        // POST: ContainerTrackings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AccessAuthorizeAttribute(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            ContainerTracking containerTracking = db.ContainerTracking.Find(id);
            db.ContainerTracking.Remove(containerTracking);
            db.SaveChanges();

            MyLogger.GetInstance.Info(Resources.Resources.DeletedText + " ContainerTrackingId: " + containerTracking.ContainerTrackingId + " ContainerNumber: " + containerTracking.ContainerNumber);

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
