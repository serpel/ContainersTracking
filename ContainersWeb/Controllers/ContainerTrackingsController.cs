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

        public ActionResult GetCompanyByNumber(string number)
        {
            var result = db.ContainerTracking.Where(w => w.ContainerNumber == number).ToList().OrderByDescending(o => o.InsertedAt).Take(1).Select(s => new { s.CompanyOriginId, Name = s.CompanyDestination.Name }).FirstOrDefault();

            if( result != null)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetContainerTrackings()
        {
            var containers = db.ContainerTracking
                .ToList()
                .Where(w => w.InsertedAt > DateTime.Now.AddDays(-20))
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

        public ActionResult Move()
        {
            var record = new MoveViewModel();

            record.Date = DateTime.Now;
            record.User = User.Identity.Name;

            ViewBag.Number = new SelectList(GetContainersIn().Where(w => w.CompanyDestinationId > 0 && w.CompanyOriginId > 0), "ContainerNumber", "ContainerNumber");
            ViewBag.CompanyDestinationId = new SelectList(db.Companies.Where(w => w.IsActive == true), "CompanyId", "Name");
            //ViewBag.CompanyOriginId = new SelectList(db.Companies.Where(w => w.IsActive == true), "CompanyId", "Name");

            return PartialView("Move", record);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Move([Bind(Include = "Number,CompanyOriginId,CompanyDestinationId,Date,User")] MoveViewModel record)
        {
            ContainerTracking tmp = db.ContainerTracking.Where(w => w.ContainerNumber == record.Number).OrderByDescending(d => d.InsertedAt).Take(1).FirstOrDefault();

            //if (ModelState.IsValid) {

                if (tmp != null)
                {

                    ContainerTracking recordOut = new ContainerTracking()
                    {
                        CompanyOriginId = tmp.CompanyDestinationId,
                        CompanyDestinationId = record.CompanyDestinationId,
                        ChasisNumber = tmp.ChasisNumber,
                        ContainerLabel = tmp.ContainerLabel,
                        ContainerNumber = tmp.ContainerNumber,
                        ContainerStatus = tmp.ContainerStatus,
                        ContainerLicensePlate = tmp.ContainerLicensePlate,
                        DriverId = tmp.DriverId,
                        DocNumber = tmp.DocNumber,
                        UpdatedAt = DateTime.Now,
                        InsertedAt = DateTime.Now,
                        InsertedBy = User.Identity.Name,
                        DocStatus = tmp.DocStatus,
                        Type = Models.Type.Salida,
                        CorrelAduana = tmp.CorrelAduana
                    };

                    ContainerTracking recordIn = new ContainerTracking()
                    {
                        CompanyOriginId = tmp.CompanyDestinationId,
                        CompanyDestinationId = record.CompanyDestinationId,
                        ChasisNumber = tmp.ChasisNumber,
                        ContainerLabel = tmp.ContainerLabel,
                        ContainerNumber = tmp.ContainerNumber,
                        ContainerStatus = tmp.ContainerStatus,
                        ContainerLicensePlate = tmp.ContainerLicensePlate,
                        DriverId = tmp.DriverId,
                        DocNumber = tmp.DocNumber,
                        UpdatedAt = DateTime.Now,
                        InsertedAt = DateTime.Now,
                        InsertedBy = User.Identity.Name,
                        DocStatus = tmp.DocStatus,
                        Type = Models.Type.Entrada,
                        CorrelAduana = tmp.CorrelAduana
                    };

                    db.ContainerTracking.Add(recordOut);
                    db.ContainerTracking.Add(recordIn);

                    db.SaveChanges();

                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            //}

            ViewBag.Number = new SelectList(db.ContainerTracking.Where(w => w.CompanyOriginId > 0 && w.CompanyDestinationId > 0 && w.Type == Models.Type.Entrada).GroupBy(x => x.ContainerNumber).Select(g => g.FirstOrDefault()), "ContainerNumber", "ContainerNumber");
            ViewBag.CompanyDestinationId = new SelectList(db.Companies.Where(w => w.IsActive == true), "CompanyId", "Name");

            return PartialView("Move", record);
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
            containerTracking.DocStatus = DocStatus.Pendiente;

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

        public ActionResult In()
        {
            var container = new ContainerTracking();
            container.InsertedAt = DateTime.Now;
            container.InsertedBy = User.Identity.Name;

            return PartialView("In", container);
        }

        // POST: ContainerTrackings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult In([Bind(Include = "ContainerTrackingId,Type,CompanyOriginId,CompanyDestinationId,DocStatus,ContainerNumber,ContainerStatus,ContainerLicensePlate,ContainerLabel,ChasisNumber,DocNumber,CorrelAduana,DriverId,SecuritySupervisorId,InsertedAt,InsertedBy,UpdatedAt,UpdatedBy")] ContainerTracking containerTracking)
        {
            containerTracking.UpdatedAt = DateTime.Now;
            containerTracking.DocStatus = DocStatus.Pendiente;
            containerTracking.Type = Models.Type.Entrada;

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

            return PartialView("In", containerTracking);
        }

        public List<ContainerTracking> GetContainersIn()
        {
            var list = from d in db.ContainerTracking
                       group d by d.ContainerNumber into g
                       select new { ContainerNumber = g.Key, Id = g.Max(m => m.ContainerTrackingId) };

            var query = from c in db.ContainerTracking
                        join l in list on c.ContainerTrackingId equals l.Id
                        where c.Type == Models.Type.Entrada
                        select c;

            return query.ToList();
        }

        public ActionResult Out()
        {
            var container = new ContainerTracking();
            container.InsertedAt = DateTime.Now;
            container.InsertedBy = User.Identity.Name;

            ViewBag.ContainerNumber = new SelectList(GetContainersIn(), "ContainerNumber", "ContainerNumber");

            return PartialView("Out", container);
        }

        // POST: ContainerTrackings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Out([Bind(Include = "ContainerTrackingId,Type,CompanyOriginId,CompanyDestinationId,DocStatus,ContainerNumber,ContainerStatus,ContainerLicensePlate,ContainerLabel,ChasisNumber,DocNumber,CorrelAduana,DriverId,SecuritySupervisorId,InsertedAt,InsertedBy,UpdatedAt,UpdatedBy")] ContainerTracking containerTracking)
        {
            containerTracking.UpdatedAt = DateTime.Now;
            containerTracking.DocStatus = DocStatus.Pendiente;
            containerTracking.Type = Models.Type.Salida;

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

            ViewBag.ContainerNumber = new SelectList(GetContainersIn(), "ContainerNumber", "ContainerNumber");
            return PartialView("Out", containerTracking);
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
            ViewBag.DriverId = new SelectList(db.Drivers.Where(w => w.IsActive == true), "DriverId", "Name", containerTracking.DriverId);
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
            ViewBag.DriverId = new SelectList(db.Drivers.Where(w => w.IsActive == true), "DriverId", "Name", containerTracking.DriverId);
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
