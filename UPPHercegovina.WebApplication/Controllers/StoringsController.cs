using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Controllers
{
    public class StoringsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Storings
        public ActionResult Index()
        {
            return View(db.Storings.ToList());
        }

        // GET: Storings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Storing storing = db.Storings.Find(id);
            if (storing == null)
            {
                return HttpNotFound();
            }
            return View(storing);
        }

        // GET: Storings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Storings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Storing storing)
        {
            if (ModelState.IsValid)
            {
                db.Storings.Add(storing);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(storing);
        }

        // GET: Storings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Storing storing = db.Storings.Find(id);
            if (storing == null)
            {
                return HttpNotFound();
            }
            return View(storing);
        }

        // POST: Storings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] Storing storing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(storing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(storing);
        }

        // GET: Storings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Storing storing = db.Storings.Find(id);
            if (storing == null)
            {
                return HttpNotFound();
            }
            return View(storing);
        }

        // POST: Storings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Storing storing = db.Storings.Find(id);
            db.Storings.Remove(storing);
            db.SaveChanges();
            return RedirectToAction("Index");
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
