using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Controllers
{
    public class RolesCounterController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public RolesCounterController()
        {

        }

        public RolesCounterController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: RolesCounter
        public ActionResult Index()
        {
            var myRoles = context.Roles.Include(r => r.Users).ToList();

            return View();
        }
    }
}