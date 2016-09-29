using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using UPPHercegovina.WebApplication.CustomFilters;
using UPPHercegovina.WebApplication.Helpers;
using UPPHercegovina.WebApplication.Models;
using UPPHercegovina.WebApplication.Abstractions;

namespace UPPHercegovina.WebApplication.Controllers
{
    [AuthLog(Roles = "Super-Administrator, Administrator")]
    public class MembershipsController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        public ActionResult Index(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.StatusSortParm = sortOrder == "Status" ? "status_desc" : "Status";

            var memberships = context.Memberships.ToList();
            memberships = SortFactory.Resolve(sortOrder, memberships.GetType()).Sort(memberships);

            return memberships != null ? View(memberships.ToList()) : View(new List<Membership>());      
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var membership = context.Memberships.Find(id);
            if (membership == null)
            {
                return HttpNotFound();
            }
            return View(membership);
        }

        public ActionResult Create()
        {
            TempData["Data"] = DateTime.Now.ToString();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description, Price,CreationDate,Status")] Membership membership)
        {
            TempData["Data"] = DateTime.Now.ToString();
            if (ModelState.IsValid)
            {
                context.Memberships.Add(membership);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(membership);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var membership = context.Memberships.Find(id);
            if (membership == null)
            {
                return HttpNotFound();
            }
            return View(membership);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Price,CreationDate,Status")] Membership membership)
        {
            if (ModelState.IsValid)
            {
                context.Entry(membership).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(membership);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var membership = context.Memberships.Find(id);
            if (membership == null)
            {
                return HttpNotFound();
            }
            return View(membership);
        }

        public ActionResult Deactivate(int? id)
        {
            var membership = context.Memberships.Find(id);

            if(membership == null)
            {
                //bacit exception ali za sada ovo
                return RedirectToAction("Index");
            }

            membership.Status = false;

            context.Memberships.Attach(membership);
            context.Entry(membership).State = EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var membership = context.Memberships.Find(id);

            if(membership == null)
                return RedirectToAction("Index");

            context.Memberships.Remove(membership);
            context.SaveChanges();
            return RedirectToAction("Index");
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
