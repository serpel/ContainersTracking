using ContainersWeb.DAL.Security;
using ContainersWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContainersWeb.Controllers
{
    [AccessAuthorizeAttribute(Roles = "Admin, Manager, User")]
    public class HomeController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var today = DateTime.Now;
            var list = db.ContainerTracking.Where(w => w.InsertedAt.Day == today.Day &&
                                              w.InsertedAt.Month == today.Month &&
                                              w.InsertedAt.Year == today.Year &&
                                              w.IsInternalMove == false).ToList();

            ViewBag.TruckPending = list.Where(w => w.DocStatus == DocStatus.Pendiente
                                                && w.TrackingType == TrackingType.Camion).Count();
            ViewBag.TruckReady = list.Where(w => w.DocStatus == DocStatus.Listo
                                              && w.TrackingType == TrackingType.Camion).Count();
            ViewBag.ContainersPending = list.Where(w => w.DocStatus == DocStatus.Pendiente
                                                && w.TrackingType == TrackingType.Contenedor).Count();
            ViewBag.ContainersReady = list.Where(w => w.DocStatus == DocStatus.Listo
                                              && w.TrackingType == TrackingType.Contenedor).Count();
            ViewBag.RastraPending = list.Where(w => w.DocStatus == DocStatus.Pendiente
                                            && w.TrackingType == TrackingType.Rastra).Count();
            ViewBag.RastraReady = list.Where(w => w.DocStatus == DocStatus.Listo
                                          && w.TrackingType == TrackingType.Rastra).Count();
            ViewBag.VehiclePending = list.Where(w => w.DocStatus == DocStatus.Pendiente
                                           && w.TrackingType == TrackingType.Vehiculo).Count();
            ViewBag.VehicleReady = list.Where(w => w.DocStatus == DocStatus.Listo
                                          && w.TrackingType == TrackingType.Vehiculo).Count();
            ViewBag.MotoPending = list.Where(w => w.DocStatus == DocStatus.Pendiente
                                           && w.TrackingType == TrackingType.Moto).Count();
            ViewBag.MotoReady = list.Where(w => w.DocStatus == DocStatus.Listo
                                          && w.TrackingType == TrackingType.Moto).Count();
            ViewBag.CourierPending = list.Where(w => w.DocStatus == DocStatus.Pendiente
                                          && w.TrackingType == TrackingType.Courier).Count();
            ViewBag.CourierReady = list.Where(w => w.DocStatus == DocStatus.Listo
                                          && w.TrackingType == TrackingType.Courier).Count();

            ViewBag.Trucks = list.Where(w => w.TrackingType == TrackingType.Camion).Count();
            ViewBag.Containers = list.Where(w => w.TrackingType == TrackingType.Contenedor).Count();
            ViewBag.Rastras = list.Where(w => w.TrackingType == TrackingType.Rastra).Count();
            ViewBag.Motos = list.Where(w => w.TrackingType == TrackingType.Moto).Count();
            ViewBag.Couriers = list.Where(w => w.TrackingType == TrackingType.Courier).Count();
            ViewBag.Vehicles = list.Where(w => w.TrackingType == TrackingType.Vehiculo).Count();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}