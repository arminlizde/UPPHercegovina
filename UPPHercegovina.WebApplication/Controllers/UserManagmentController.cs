using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UPPHercegovina.WebApplication.Models;

using UPPHercegovina.WebApplication.CustomFilters;
using UPPHercegovina.WebApplication.Helpers;
using PagedList;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace UPPHercegovina.WebApplication.Controllers
{
    [AuthLog(Roles = "Super-Administrator")]
    public class UserManagmentController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        private int _pageSize = 10;

        public UserManagmentController()
        {

        }

        public UserManagmentController(ApplicationUserManager userManager)
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


        public ActionResult AllUsers(int? page,bool status = true)
        {
           int pageNumber = (page ?? 1);

            ViewBag.Status = status;

            List<UsersViewModel> userViewList = UsersViewModel.UsersToViewModel(status);
            return userViewList != null ? View(userViewList.ToPagedList(pageNumber, _pageSize)) : View(new List<UsersViewModel>().ToPagedList(pageNumber, _pageSize));
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = context.Users.Find(id);
            
            var userEditModel = new UserEditViewModel();

            if (user != null)
            {
                userEditModel = ApplicationUserHelper.MapToEditViewModel(user);

                if (String.IsNullOrWhiteSpace(userEditModel.PictureUrl))
                {
                    userEditModel.PictureUrl = "/Images/UserImg/avatarDefault.png";
                }

                var townshipList = context.PlaceOfResidences.OrderByDescending(t => t.Id == userEditModel.TownshipId).
                    ThenBy(t => t.Name).ToList();

                ViewBag.TownshipId = new SelectList(townshipList, "Id", "Name");
            }
            else
                return HttpNotFound();

            return View(userEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,IdentificationNumber,PictureUrl,TownshipId,Address,Email,PhoneNumber, EmailConfirmed")] UserEditViewModel model,
           FormCollection form, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    SavePictureToServer(file, model);
                    //neku poruku ispisatu u slucaju errora
                }

                var user = FillApplicationUser(model);

                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("AllUsers");
            }
            return View();
        }

        private ApplicationUser FillApplicationUser(UserEditViewModel model)
        {
            ApplicationUser user = context.Users.Find(model.Id);

            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.IdentificationNumber = model.IdentificationNumber;

            if (model.PictureUrl != null)
                user.PictureUrl = model.PictureUrl;

            user.TownshipId = model.TownshipId;
            user.Address = model.Address;
            user.PhoneNumber = model.PhoneNumber;

            if (user.Email != User.Identity.Name)
                user.EmailConfirmed = model.EmailConfirmed;

            return user;
        }

        private bool SavePictureToServer(HttpPostedFileBase file, UserEditViewModel model)
        {
            var path = "/Images/UserImg/" +
                model.FirstName + model.LastName + "-" + model.IdentificationNumber;

            model.PictureUrl = string.Format("{0}/Profile-{1}{2}-{3}.jpg", path, model.FirstName,
                model.LastName, Extensions.Extensions.GetRndNumber());

            PictureHelper pictureHelper = new PictureHelper(file, model.PictureUrl);

            return pictureHelper.Save();
        }

        public ActionResult Delete(string id)
        {
            var user = context.Users.Find(id);

            if (user.Email == User.Identity.Name)
            {
                //TOAST MESSAGE
                ViewBag.Error = "Ne možete obrisati sebe, niti Super-Administratora";
                return RedirectToAction("AllUsers");
            }

            context.Users.Remove(user);
            context.SaveChanges();

            return RedirectToAction("AllUsers");
        }

        public ActionResult Deactivate(string id)
        {
            var user = context.Users.Find(id);

            if (user.Email == User.Identity.Name)
            {
                ViewBag.Error = "Ne možete deaktivirati sebe, niti Super-Administratora";
                return RedirectToAction("AllUsers");
            }

            user.Status = false;
            context.Users.Attach(user);
            context.Entry(user).State = EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("AllUsers");
        }

        public ActionResult AddUser()
        {
            ViewBag.TownshipId = new SelectList(context.PlaceOfResidences, "Id", "Name");
            List<IdentityRole> roles = context.Roles.ToList();
            List<IdentityRole> rolesForView = new List<IdentityRole>();

            foreach (var item in roles)
            {
                if (item.Name == "Nakupac")
                {
                    rolesForView.Add(item);
                }
                if (item.Name == "Korisnik")
                {
                    rolesForView.Add(item);
                }
            }

            ViewBag.RoleID = new SelectList(rolesForView, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddUser(RegisterViewModel model)
        {
            List<IdentityRole> roles = context.Roles.ToList();
            List<IdentityRole> rolesForView = new List<IdentityRole>();

            foreach (var item in roles)
            {
                if (item.Name == "Nakupac")
                {
                    rolesForView.Add(item);
                }
                if (item.Name == "Korisnik")
                {
                    rolesForView.Add(item);
                }
            }

            ViewBag.TownshipId = new SelectList(context.PlaceOfResidences, "Id", "Name");
            ViewBag.RoleID = new SelectList(rolesForView, "Id", "Name");

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    RegistrationDate = DateTime.Now,
                    TownshipId = model.TownshipId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    IdentificationNumber = model.IdentificationNumber,
                    EmailConfirmed = model.EmailConfirmed,
                    Status = model.EmailConfirmed
                };

                var path = "/Images/UserImg/" +
                    user.FirstName + user.LastName + "-" + user.IdentificationNumber;
                DirectoryHelper dh = new DirectoryHelper(path);
                dh.Create();

                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {

                    var role = context.Roles.Find(model.RoleID);
                    RoleAddToUser(user, role.Name);

                    //string roleid = model.RoleID;
                    //IdentityUserRole ur = new IdentityUserRole();

                    //ur.RoleId = roleid;
                    //ur.UserId = user.Id;
                    //user.Roles.Add(ur);
                    //context.SaveChanges();


                    if (!user.EmailConfirmed)
                    {
                        string body = string.Format("Poštovani, {0} <BR/>Hvala Vam na registraciji, molimo da pratite link kako bi verifikovali Email: <a href=\"{1}\"title=\"User Email Confirm\">{1}</a>",
                        user.UserName, Url.Action("ConfirmEmail", "Account",
                            new { Token = user.Id, Email = user.Email }, Request.Url.Scheme));
                        var emailClient = new EmailClient("Potvrda Email adrese", user.Email, body);
                        emailClient.Send();
                    }

                    return RedirectToAction("AllUsers");
                }
                AddErrors(result);
            }
            return View(model);
        }

        public bool RoleAddToUser(ApplicationUser user, string RoleName)
        {            
            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            userManager.AddToRole(user.Id, RoleName);

            

            return true;
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

    }
}