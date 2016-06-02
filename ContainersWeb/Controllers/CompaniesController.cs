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
    [AccessAuthorizeAttribute(Roles = "Admin, Manager")]
    public class CompaniesController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult GetCompanies()
        {
            var companies = db.Companies
                .ToList()
                .Select(s => new { s.Name, s.IsActive, s.CompanyId, Region = s.Region.Name });

            return Json(companies, JsonRequestBehavior.AllowGet);
        }

        // GET: Companies
        public ActionResult Index()
        {
            return View();
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            ViewBag.RegionId = new SelectList(db.Regions, "RegionId", "Name");
            return PartialView("Create", new Company());
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CompanyId,Name,Code,RegionId,IsActive")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Companies.Add(company);
                db.SaveChanges();

                MyLogger.GetInstance.Info("The Company was created succesfull, Name: " + company.Name);
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }         

            ViewBag.RegionId = new SelectList(db.Regions, "RegionId", "Name", company.RegionId);
            return PartialView("Create", company);
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            ViewBag.RegionId = new SelectList(db.Regions, "RegionId", "Name", company.RegionId);
            return PartialView("Edit", company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompanyId,Name,Code,RegionId,IsActive")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();

                MyLogger.GetInstance.Info("The Company was edited succesfull, Id: " + company.CompanyId);
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.RegionId = new SelectList(db.Regions, "RegionId", "Name", company.RegionId);
            return PartialView("Edit", company);
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return PartialView("Delete", company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
            db.SaveChanges();

            MyLogger.GetInstance.Info("The Company was deleted succesfull, Id: " + id);
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
