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
                                              w.InsertedAt.Year == today.Year).ToList();

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

            ViewBag.Trucks = list.Where(w => w.TrackingType == TrackingType.Camion).Count();
            ViewBag.Containers = list.Where(w => w.TrackingType == TrackingType.Contenedor).Count();
            ViewBag.Rastras = list.Where(w => w.TrackingType == TrackingType.Rastra).Count();

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