using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UPPHercegovina.WebApplication.CustomFilters;

using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Controllers
{
    [AuthLog(Roles = "Super-Administrator, Administrator")]
    public class PersonProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PersonProducts
        public ActionResult Index()
        {
            return View(db.PersonProducts.ToList());
        }

        // GET: PersonProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonProduct personProduct = db.PersonProducts.Find(id);
            if (personProduct == null)
            {
                return HttpNotFound();
            }
            return View(personProduct);
        }

        // GET: PersonProducts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PersonProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,ProductId,HarvestDate,Neto,Bruto,Coordinates,ExparationDate,StoringId,Quality,WarehouseId")] PersonProduct personProduct)
        {
            if (ModelState.IsValid)
            {
                db.PersonProducts.Add(personProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(personProduct);
        }

        // GET: PersonProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonProduct personProduct = db.PersonProducts.Find(id);
            if (personProduct == null)
            {
                return HttpNotFound();
            }
            return View(personProduct);
        }

        // POST: PersonProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,ProductId,HarvestDate,Neto,Bruto,Coordinates,ExparationDate,StoringId,Quality,WarehouseId")] PersonProduct personProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(personProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(personProduct);
        }

        // GET: PersonProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonProduct personProduct = db.PersonProducts.Find(id);
            if (personProduct == null)
            {
                return HttpNotFound();
            }
            return View(personProduct);
        }

        // POST: PersonProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PersonProduct personProduct = db.PersonProducts.Find(id);
            db.PersonProducts.Remove(personProduct);
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
