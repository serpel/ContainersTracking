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

        public ActionResult GetExternalCompanies()
        {
            var result = db.Companies.Where(w => w.Region.Name.Contains("Zona Externa")).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetIntenalCompanies()
        {
            var result = db.Companies.Where(w => !w.Region.Name.Contains("Zona Externa")).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSecurities()
        {
            var result = db.SecuritySupervisors.Where(w => w.IsActive == true).ToList().Select(s => new { s.SecuritySupervisorId, s.Name });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDrivers()
        {
            var result = db.Drivers.Where(w => w.IsActive == true).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetContainerTrackings()
        {
            var containers = db.ContainerTracking
                .ToList()
                //.Where(w => w.InsertedAt > DateTime.Now.AddDays(-20))
                .Select(s => new { s.ContainerTrackingId, Type = s.Type == 0 ? Resources.Resources.Out: Resources.Resources.In,
                    DocStatus = s.DocStatus == 0 ? Resources.Resources.Pending : Resources.Resources.Ready,
                    ContainerStatus = s.ContainerStatus == 0 ? Resources.Resources.Empty : Resources.Resources.Full,
                    Date = s.InsertedAt.ToString("dd-MM-yyyy HH:mm"),
                    s.ContainerNumber,
                    s.ContainerLicensePlate,
                    Tracking = s.TrackingType == TrackingType.Contenedor ? Resources.Resources.Container :
                                   s.TrackingType == TrackingType.Camion ? Resources.Resources.Truck :
                                   s.TrackingType == TrackingType.Rastra ? Resources.Resources.Rastra : "" 
                });

            return Json(containers, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetContainerTrackingFilter(DTParameters param)
        {
            try
            {
                List<ContainerTrackingSearchViewModel> dtsource = db.ContainerTracking.ToList().Select(s => new ContainerTrackingSearchViewModel() {
                    ContainerTrackingId = s.ContainerTrackingId,
                    ContainerNumber = s.ContainerNumber,
                    ContainerLicensePlate = s.ContainerLicensePlate,
                    ContainerStatus = s.ContainerStatus == 0 ? Resources.Resources.Empty : Resources.Resources.Full,
                    TrackingType = s.TrackingType == TrackingType.Contenedor ? Resources.Resources.Container :
                                   s.TrackingType == TrackingType.Camion ? Resources.Resources.Truck :
                                   s.TrackingType == TrackingType.Rastra ? Resources.Resources.Rastra : "",
                    DocStatus = s.DocStatus == 0 ? Resources.Resources.Pending : Resources.Resources.Ready,
                    Type = s.Type == 0 ? Resources.Resources.Out : Resources.Resources.In,
                    InsertedAt = s.InsertedAt.ToString("dd-MM-yyyy HH:mm"),
                    UpdatedAt = s.UpdatedAt.ToString("dd-MM-yyyy HH:mm")
                }).ToList();       

                List<String> columnSearch = new List<string>();

                foreach (var col in param.Columns)
                {
                    columnSearch.Add(col.Search.Value);
                }

                List<ContainerTrackingSearchViewModel> data = new ResultSet().GetResult(param.Search.Value, param.SortOrder, param.Start, param.Length, dtsource, columnSearch);
                int count = new ResultSet().Count(param.Search.Value, dtsource, columnSearch);
                DTResult<ContainerTrackingSearchViewModel> result = new DTResult<ContainerTrackingSearchViewModel>
                {
                    draw = param.Draw,
                    data = data,
                    recordsFiltered = count,
                    recordsTotal = count
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
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
            var region = db.Regions.Where(w => w.Name.Contains("Zona Externa")).FirstOrDefault();
            ViewBag.CompanyDestinationId = new SelectList(db.Companies.Where(w => w.IsActive == true && w.RegionId != region.RegionId), "CompanyId", "Name");

            var regions = db.Regions.Where(w => w.RegionId != region.RegionId);
            ViewBag.GateIn = new SelectList(regions, "RegionId", "Name");
            ViewBag.GateOut = new SelectList(regions, "RegionId", "Name");

            return PartialView("Move", record);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Move([Bind(Include = "Number,CompanyOriginId,CompanyDestinationId,Date,User,GateIn,GateOut")] MoveViewModel record)
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
                        GateId = record.GateOut,
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
                        GateId = record.GateIn,
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
        public ActionResult Create([Bind(Include = "ContainerTrackingId,Type,CompanyOriginId,CompanyDestinationId,DocStatus,ContainerNumber,ContainerStatus,ContainerLicensePlate,ContainerLabel,ChasisNumber,DocNumber,CorrelAduana,DriverId,SecuritySupervisorId,InsertedAt,InsertedBy,UpdatedAt,UpdatedBy,TrackingType")] ContainerTracking containerTracking)
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

            ViewBag.GateId = new SelectList(db.Regions.Where(w => !w.Name.Contains("Zona Externa")), "RegionId", "Name");
            return PartialView("In", container);
        }

        // POST: ContainerTrackings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult In([Bind(Include = "ContainerTrackingId,Type,CompanyOriginId,CompanyDestinationId,DocStatus,ContainerNumber,ContainerStatus,ContainerLicensePlate,ContainerLabel,ChasisNumber,DocNumber,CorrelAduana,DriverId,SecuritySupervisorId,InsertedAt,InsertedBy,UpdatedAt,UpdatedBy,GateId,TrackingType")] ContainerTracking containerTracking)
        {
            containerTracking.UpdatedAt = DateTime.Now;
            containerTracking.DocStatus = DocStatus.Pendiente;
            containerTracking.Type = Models.Type.Entrada;

            ContainerTrackingHelper validator = new ContainerTrackingHelper(db);

            if (ModelState.IsValid)
            {
                if (containerTracking.TrackingType == TrackingType.Camion)
                {
                    containerTracking.ContainerLicensePlate = containerTracking.ContainerNumber;
                }

                if (validator.ValidateOnCreate(containerTracking))
                {
                    db.ContainerTracking.Add(containerTracking);
                    db.SaveChanges();

                    MyLogger.GetInstance.Info(Resources.Resources.CreatedText + " Id: " + containerTracking.ContainerTrackingId + " ContainerNumber: " + containerTracking.ContainerNumber);

                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ModelState.AddModelError("ContainerNumber", validator.Message);
                }
            }
            ViewBag.GateId = new SelectList(db.Regions.Where(w => !w.Name.Contains("Zona Externa")), "RegionId", "Name");
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
            ViewBag.GateId = new SelectList(db.Regions.Where(w => !w.Name.Contains("Zona Externa")), "RegionId", "Name");

            return PartialView("Out", container);
        }

        public ActionResult Out2()
        {
            var containerTracking = new ContainerTracking();
            containerTracking.InsertedAt = DateTime.Now;
            containerTracking.InsertedBy = User.Identity.Name;
            containerTracking.UpdatedAt = DateTime.Now;
            containerTracking.UpdatedBy = User.Identity.Name;

            ViewBag.ContainerNumber = new SelectList(GetContainersIn(), "ContainerNumber", "ContainerNumber");
            ViewBag.GateId = new SelectList(db.Regions.Where(w => !w.Name.Contains("Zona Externa")), "RegionId", "Name");

            var companiesDestination = db.Companies.Where(w => w.IsActive == true);
            var companiesOrigin = companiesDestination;
            var region = db.Regions.Where(w => w.Name.Contains("Zona Externa")).FirstOrDefault();

            if (containerTracking.Type == Models.Type.Entrada)
            {
                companiesDestination = companiesDestination.Where(w => w.RegionId != region.RegionId);
            }
            else
            {
                companiesOrigin = companiesOrigin.Where(w => w.RegionId != region.RegionId);
            }

            ViewBag.CompanyDestinationId = new SelectList(companiesDestination, "CompanyId", "Name", containerTracking.CompanyDestinationId);
            ViewBag.CompanyOriginId = new SelectList(companiesOrigin, "CompanyId", "Name", containerTracking.CompanyOriginId);
            ViewBag.DriverId = new SelectList(db.Drivers.Where(w => w.IsActive == true), "DriverId", "Name", containerTracking.DriverId);
            ViewBag.SecuritySupervisorId = new SelectList(db.SecuritySupervisors.Where(w => w.IsActive == true), "SecuritySupervisorId", "Name", containerTracking.SecuritySupervisorId);

            return PartialView("Out", containerTracking);
        }


        // POST: ContainerTrackings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Out2([Bind(Include = "ContainerTrackingId,Type,CompanyOriginId,CompanyDestinationId,DocStatus,ContainerNumber,ContainerStatus,ContainerLicensePlate,ContainerLabel,ChasisNumber,DocNumber,CorrelAduana,DriverId,SecuritySupervisorId,InsertedAt,InsertedBy,UpdatedAt,UpdatedBy,GateId,TrackingType")] ContainerTracking containerTracking)
        {
            containerTracking.UpdatedAt = DateTime.Now;
            containerTracking.DocStatus = DocStatus.Pendiente;
            containerTracking.Type = Models.Type.Salida;

            ContainerTrackingHelper validator = new ContainerTrackingHelper(db);

            if (ModelState.IsValid)
            {
                if (validator.ValidateOnOut(containerTracking))
                {
                    db.ContainerTracking.Add(containerTracking);
                    db.SaveChanges();

                    MyLogger.GetInstance.Info(Resources.Resources.ExitText + " Id: " + containerTracking.ContainerTrackingId + " ContainerNumber: " + containerTracking.ContainerNumber);

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

        // POST: ContainerTrackings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Out([Bind(Include = "ContainerTrackingId,Type,CompanyOriginId,CompanyDestinationId,DocStatus,ContainerNumber,ContainerStatus,ContainerLicensePlate,ContainerLabel,ChasisNumber,DocNumber,CorrelAduana,DriverId,SecuritySupervisorId,InsertedAt,InsertedBy,UpdatedAt,UpdatedBy,GateId,TrackingType")] ContainerTracking containerTracking)
        {
            var tmp = db.ContainerTracking.Where(w => w.ContainerNumber == containerTracking.ContainerNumber).OrderByDescending(o => o.ContainerTrackingId).FirstOrDefault();

            if(tmp != null)
            {
                containerTracking.TrackingType = tmp.TrackingType;
                containerTracking.Type = tmp.Type;
                containerTracking.ContainerStatus = tmp.ContainerStatus;

                if(containerTracking.TrackingType == TrackingType.Camion) { 
                    containerTracking.ContainerLicensePlate = tmp.ContainerNumber;
                }
            }

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

                    MyLogger.GetInstance.Info(Resources.Resources.ExitText + " Id: " + containerTracking.ContainerTrackingId + " ContainerNumber: " + containerTracking.ContainerNumber);

                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ModelState.AddModelError("ContainerNumber", validator.Message);
                }
            }
            ViewBag.GateId = new SelectList(db.Regions.Where(w => !w.Name.Contains("Zona Externa")), "RegionId", "Name");
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

            var companiesDestination = db.Companies.Where(w => w.IsActive == true);
            var companiesOrigin = companiesDestination;
            var region = db.Regions.Where(w => w.Name.Contains("Zona Externa")).FirstOrDefault();

            if (containerTracking.Type == Models.Type.Entrada)
            {              
                companiesDestination = companiesDestination.Where(w => w.RegionId != region.RegionId);
            }else
            {
                companiesOrigin = companiesOrigin.Where(w => w.RegionId != region.RegionId);
            }

            ViewBag.GateId = new SelectList(db.Regions.Where(w => !w.Name.Contains("Zona Externa")), "RegionId", "Name", containerTracking.GateId);
            ViewBag.CompanyDestinationId = new SelectList(companiesDestination, "CompanyId", "Name", containerTracking.CompanyDestinationId);
            ViewBag.CompanyOriginId = new SelectList(companiesOrigin, "CompanyId", "Name", containerTracking.CompanyOriginId);
            ViewBag.DriverId = new SelectList(db.Drivers.Where(w => w.IsActive == true), "DriverId", "Name", containerTracking.DriverId);
            ViewBag.SecuritySupervisorId = new SelectList(db.SecuritySupervisors.Where(w => w.IsActive == true), "SecuritySupervisorId", "Name", containerTracking.SecuritySupervisorId);
            return PartialView("Edit", containerTracking);
        }

        // POST: ContainerTrackings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContainerTrackingId,Type,CompanyOriginId,CompanyDestinationId,DocStatus,ContainerNumber,ContainerStatus,ContainerLicensePlate,ContainerLabel,ChasisNumber,DocNumber,CorrelAduana,DriverId,SecuritySupervisorId,InsertedAt,InsertedBy,UpdatedAt,UpdatedBy,GateId,DUA,TrackingType")] ContainerTracking containerTracking)
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

        // GET: ContainerTrackings/Edit/5
        [AccessAuthorizeAttribute(Roles = "Admin")]
        public ActionResult EditDocument(int? id)
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

            var companiesDestination = db.Companies.Where(w => w.IsActive == true);
            var companiesOrigin = companiesDestination;
            var region = db.Regions.Where(w => w.Name.Contains("Zona Externa")).FirstOrDefault();

            if (containerTracking.Type == Models.Type.Entrada)
            {
                companiesDestination = companiesDestination.Where(w => w.RegionId != region.RegionId);
            }
            else
            {
                companiesOrigin = companiesOrigin.Where(w => w.RegionId != region.RegionId);
            }

            ViewBag.GateId = new SelectList(db.Regions.Where(w => !w.Name.Contains("Zona Externa")), "RegionId", "Name", containerTracking.GateId);
            ViewBag.CompanyDestinationId = new SelectList(companiesDestination, "CompanyId", "Name", containerTracking.CompanyDestinationId);
            ViewBag.CompanyOriginId = new SelectList(companiesOrigin, "CompanyId", "Name", containerTracking.CompanyOriginId);
            ViewBag.DriverId = new SelectList(db.Drivers.Where(w => w.IsActive == true), "DriverId", "Name", containerTracking.DriverId);
            ViewBag.SecuritySupervisorId = new SelectList(db.SecuritySupervisors.Where(w => w.IsActive == true), "SecuritySupervisorId", "Name", containerTracking.SecuritySupervisorId);
            return PartialView("Edit", containerTracking);
        }

        // POST: ContainerTrackings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessAuthorizeAttribute(Roles = "Admin")]
        public ActionResult EditDocument([Bind(Include = "ContainerTrackingId,Type,CompanyOriginId,CompanyDestinationId,DocStatus,ContainerNumber,ContainerStatus,ContainerLicensePlate,ContainerLabel,ChasisNumber,DocNumber,CorrelAduana,DriverId,SecuritySupervisorId,InsertedAt,InsertedBy,UpdatedAt,UpdatedBy,GateId,DUA,TrackingType")] ContainerTracking containerTracking)
        {
            containerTracking.UpdatedAt = DateTime.Now;

            ContainerTrackingHelper validator = new ContainerTrackingHelper(db);

            if (ModelState.IsValid)
            {
                if (validator.ValidateOnEdit(containerTracking))
                {
                    db.Entry(containerTracking).State = EntityState.Modified;
                    db.SaveChanges();

                    MyLogger.GetInstance.Info(Resources.Resources.EditText + " ContainerTrackingId: " + containerTracking.ContainerTrackingId + " ContainerNumber: " + containerTracking.ContainerNumber);

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
