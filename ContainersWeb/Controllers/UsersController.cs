﻿using ContainersWeb.BLL;
using ContainersWeb.DAL.Security;
using ContainersWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ContainersWeb.Controllers
{
    [AccessAuthorizeAttribute(Roles = "Admin")]
    public class UsersController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                if (_userManager == null)
                {
                    _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                }
                return _userManager;
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetUsers()
        {

            var result = db.Users
                        .Select(s => new { s.Id, s.UserName, s.Email, s.EmailConfirmed });

            return Json(result.ToList(), JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(string id)
        {
            var user = UserManager.FindById(id);

            string[] selectedRoles = user.Roles.Select(x => x.RoleId).ToArray();

            ViewBag.Roles = new MultiSelectList(db.Roles.ToList(), "Id", "Name", null, selectedRoles);

            UserViewModel userV = new UserViewModel() { Id = user.Id, Email = user.Email };
                
            return PartialView("Edit", userV);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserViewModel model)
        {
            //TODO: Fix error when delete all companies
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(model.Id);

                var roleStore = new RoleStore<IdentityRole>(db);
                var roleManager = new RoleManager<IdentityRole>(roleStore);

                var rolesToDelete = user.Roles.Select(s => s.RoleId).Except(model.Roles).ToList();
                var rolesToAdd = model.Roles.Except(user.Roles.Select(s => s.RoleId)).ToList();

                if (rolesToDelete.Count > 0)
                {
                    string[] roles = db.Roles
                        .Where(w => rolesToDelete.Contains(w.Id))
                        .Select(s => s.Name)
                        .ToArray();

                    await UserManager.RemoveFromRolesAsync(user.Id, roles);
                }

                if (rolesToAdd.Count > 0)
                {
                    string[] newRoles = db.Roles
                        .Where(w => rolesToAdd.Contains(w.Id))
                        .Select(s => s.Name)
                        .ToArray();

                    await UserManager.AddToRolesAsync(user.Id, newRoles);
                }             

                MyLogger.GetInstance.Info("User was edited Succesfull, userId: " + user.Id + " Email: " + user.Email);

                //TODO: save company user
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            ViewBag.Roles = new SelectList(db.Roles.ToList(), "Id", "Name");
            ViewBag.Companies = new SelectList(db.Companies.ToList(), "CompanyId", "Name");

            return PartialView("Edit", model);
        }

        public ActionResult Delete(string id)
        {
            var user = UserManager.FindById(id);
            UserViewModel userV = new UserViewModel() { Id = user.Id, Email = user.Email };

            return PartialView("Delete", userV);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(UserViewModel model)
        {
            var user = await UserManager.FindByIdAsync(model.Id);
            var logins = user.Logins;

            foreach (var login in logins.ToList())
            {
                await UserManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
            }

            var rolesForUser = await UserManager.GetRolesAsync(user.Id);

            foreach (var item in rolesForUser.ToList())
            {
                await UserManager.RemoveFromRoleAsync(user.Id, item);
            }

            var userClaims = await UserManager.GetClaimsAsync(user.Id);

            foreach (var item in userClaims.ToList())
            {
                await UserManager.RemoveClaimAsync(user.Id, item);
            }

            var result = await UserManager.DeleteAsync(user);

            MyLogger.GetInstance.Info("User was delted Succesfull, userId: " + user.Id + " Email: " + user.Email);

            return Json(new { success = result.Succeeded }, JsonRequestBehavior.AllowGet);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        //
        // GET: /Account/Register
        public ActionResult Create()
        {
            return PartialView("Create");
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email this link
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    MyLogger.GetInstance.Info("User was created Succesfull, userId: " + user.Id + " Email: " + user.Email);

                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return PartialView("Create", model);
        }

        public ActionResult UserProfile()
        {
            return View();
        }

        //public ActionResult UserProfile()
        //{
        //    return 
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}