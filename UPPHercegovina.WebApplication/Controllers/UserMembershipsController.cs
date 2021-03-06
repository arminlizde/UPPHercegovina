﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UPPHercegovina.WebApplication.CustomFilters;
using UPPHercegovina.WebApplication.Helpers;
using UPPHercegovina.WebApplication.Helpers.UserMembershipHelper;
using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Controllers
{
    [AuthLog(Roles = "Super-Administrator, Administrator")]
    public class UserMembershipsController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        public ActionResult Index(string sortOrder)
        {
            ViewBag.StatusSortParm = sortOrder == "status" ? "status_desc" : "status";
            ViewBag.ApprovedSortParm = sortOrder == "approved" ? "approved_desc" : "approved";
            ViewBag.MembershipSortParm = sortOrder == "membership" ? "membership_desc" : "membership";
            ViewBag.DateSortParm = sortOrder == "date" ? "date_desc" : "date";
            ViewBag.UserSortParm = String.IsNullOrEmpty(sortOrder) ? "user_desc" : "";

            var usermemberships = context.UserMemberships.Include(um => um.User).Include(um => um.Membership).ToList();
            var modellist = Mapper.MapTo<List<UserMembershipIndexModel>, List<UserMembership>>(usermemberships);
            modellist = SortFactory.Resolve(sortOrder, modellist.GetType()).Sort(modellist);
            
            return View(modellist);
        }
      
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userMembership = context.UserMemberships.Find(id);

            if (userMembership == null)
            {
                return HttpNotFound();
            }
            return View(userMembership);
        }

        public ActionResult Create()
        {
            var userSelectList = ApplicationUserHelper.GetUserSelectList();

            ViewBag.UserId = new SelectList(userSelectList, "UserId", "Name");
            ViewBag.MembershipId = new SelectList(context.Memberships.OrderBy(m => m.CreationDate), "Id", "Name");
            ViewData["Date"] = DateTime.Now;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MembershipId,UserId,DateOfPayment,PictureUrl,Approved, Status")] UserMembershipViewModel userMembership,
            FormCollection form, HttpPostedFileBase file)
        {
            var usermembership = Mapper.MapTo<UserMembership, UserMembershipViewModel>(userMembership);

            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    if (SavePictureToServer(file, usermembership))
                    {
                        context.UserMemberships.Add(usermembership);
                        context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.message = "Please choose only Image file";
                        return RedirectToAction("Index");
                    }
                }
            }
            ViewBag.MembershipId = new SelectList(context.Memberships.OrderBy(m => m.CreationDate), "Id", "Name");
            return View(userMembership);
        }

        private bool SavePictureToServer(HttpPostedFileBase file, UserMembership U)
        {
            var user = context.Users.Find(U.UserId);

            var path = string.Format("/Images/UserImg/{0}{1}-{2}/Payment", user.FirstName, 
                user.LastName, user.IdentificationNumber);

            DirectoryHelper dh = new DirectoryHelper(path);
            dh.Create();

            U.PictureUrl = string.Format("{0}/m{1}d{2}.jpg", path, U.MembershipId,
                U.DateOfPayment.ToString("dd-M-yyyy"));

            PictureHelper pictureHelper = new PictureHelper(file, U.PictureUrl);

            return pictureHelper.Save();
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userMembership = context.UserMemberships
                .Include(um => um.User)
                .Include(um => um.Membership)
                .Where(um => um.Id == id).FirstOrDefault();

            if (String.IsNullOrWhiteSpace(userMembership.PictureUrl))
            {
                userMembership.PictureUrl = "/Images/Hercegovina_grb.png";
            }
            if (userMembership == null)
            {
                return HttpNotFound();
            }

            var usermembershipEditModel = Mapper.MapTo<UserMembershipEditViewModel, UserMembership>(userMembership);

            return View(usermembershipEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MembershipId,UserId,DateOfPayment,ExpDate,PictureUrl,Approved, Status")] UserMembershipEditViewModel userMembership,
            FormCollection form, HttpPostedFileBase file)
        {
            var usermembership = context.UserMemberships.Find(userMembership.Id);
            usermembership.DateOfPayment = userMembership.DateOfPayment;
            usermembership.Approved = userMembership.Approved;
            usermembership.Status = userMembership.Status;

            if (ModelState.IsValid)
            {
                if (file != null && !SavePictureToServer(file, usermembership))
                {
                     ViewBag.message = "Please choose only Image file";
                     return RedirectToAction("Index");
                }

                context.Entry(usermembership).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userMembership);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userMembership = context.UserMemberships.Find(id);
            if (userMembership == null)
            {
                return HttpNotFound();
            }
            return View(userMembership);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var userMembership = context.UserMemberships.Find(id);
            context.UserMemberships.Remove(userMembership);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Stats(FormCollection form)
        {
            int searchIndex = 0;

            if (form.Count > 0)
            {
                var test = form.GetValue("Memberships");
                searchIndex = Convert.ToInt32(test.AttemptedValue);
            }

            var memberships = context.Memberships.ToList();
            memberships.Insert(0, new Membership()
            {
                Id = 0,
                Name = "---SVI---",
                Description = "",
                Price = 0,
                Status = true,
                CreationDate = DateTime.Now
            });

            ViewBag.Memberships = new SelectList(memberships, "Id", "Name");

            var userMemberShipList = 
                (from um in context.UserMemberships
                     where um.MembershipId == searchIndex || searchIndex == 0
                       select um).OrderBy(um => um.UserId).ThenByDescending(um => um.DateOfPayment).ToList();

            ViewBag.MembersCount = userMemberShipList.Count();

            return View(UserMembershipStatsViewModel.CreateStatsViewModelList(userMemberShipList));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
