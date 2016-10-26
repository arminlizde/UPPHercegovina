using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using UPPHercegovina.WebApplication.CustomFilters;

using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Controllers
{

    [AuthLog(Roles = "Super-Administrator, Administrator")]
    public class StoringsController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private int _pageSize = 10;

        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);

            return View(context.Storings.ToList().ToPagedList(pageNumber, _pageSize));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Storing storing = context.Storings.Find(id);
            if (storing == null)
            {
                return HttpNotFound();
            }
            return View(storing);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Storing storing)
        {
            if (ModelState.IsValid)
            {
                context.Storings.Add(storing);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(storing);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Storing storing = context.Storings.Find(id);
            if (storing == null)
            {
                return HttpNotFound();
            }
            return View(storing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] Storing storing)
        {
            if (ModelState.IsValid)
            {
                context.Entry(storing).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(storing);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Storing storing = context.Storings.Find(id);
            if (storing == null)
            {
                return HttpNotFound();
            }
            return View(storing);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Storing storing = context.Storings.Find(id);
            context.Storings.Remove(storing);
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
