using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using UPPHercegovina.WebApplication.Models;
using UPPHercegovina.WebApplication.CustomFilters;

namespace UPPHercegovina.WebApplication.Controllers
{
    [AuthLog(Roles = "Super-Administrator")]
    public class RolesController : Controller
    {
        ApplicationDbContext context;

        public RolesController()
        {
            context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            var roles = context.Roles.ToList();
            return roles != null ?  View(roles) : View(new List<IdentityRole>());
        }

        public ActionResult Create()
        {
            return View(new IdentityRole());
        }

        [HttpPost]
        public ActionResult Create(IdentityRole Role)
        {
            context.Roles.Add(Role);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string RoleName)
        {
            var thisRole = context.Roles.Where(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            context.Roles.Remove(thisRole);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ManageUserRoles()
        {           
            var roleList = context.Roles.OrderBy(r => r.Name).ToList().Select(rr =>

            new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = roleList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(string UserName, string RoleName)
        {
            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            userManager.AddToRole(user.Id, RoleName);

            ViewBag.ResultMessage = "Role created successfully !";

            var roleList = context.Roles.OrderBy(r => r.Name).ToList()
                .Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();

            ViewBag.Roles = roleList;

            return View("ManageUserRoles");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                
                var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
                ViewBag.RolesForThisUser = userManager.GetRoles(user.Id);
             
                var roleList = context.Roles.OrderBy(r => r.Name).
                    ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();

                ViewBag.Roles = roleList;
            }

            return View("ManageUserRoles");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string UserName, string RoleName)
        {
            if (UserName.Equals(User.Identity.Name) && RoleName.Equals("Super-Administrator"))
            {
                ViewBag.ResultMessage = "Ne možete sebi oduzeti Super-Administratora !";
            }
            else
            {
                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();


                if (userManager.IsInRole(user.Id, RoleName))
                {
                    userManager.RemoveFromRole(user.Id, RoleName);
                    ViewBag.ResultMessage = "Uspješno otklonjena rola od korisnika !";
                }
                else
                {
                    ViewBag.ResultMessage = "Korisnik ne pripada odabranoj roli !";
                }
            }
            var roleList = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = roleList;

            return View("ManageUserRoles");
        }
    }
}