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

namespace UPPHercegovina.WebApplication.Controllers
{
    [AuthLog(Roles = "Super-Administrator")]
    public class UserManagmentController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private int _pageSize = 10;

        public ActionResult AllUsers(int? page,bool status = false)
        {
            int pageNumber = (page ?? 1);

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

    }
}