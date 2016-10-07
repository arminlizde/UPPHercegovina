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
    public class PersonProductsController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View(context.PersonProducts.ToList());
        }

        // GET: PersonProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonProduct personProduct = context.PersonProducts.Find(id);
            if (personProduct == null)
            {
                return HttpNotFound();
            }
            return View(personProduct);
        }

        public ActionResult Create()
        {
            ViewBag.ProductId = new SelectList(context.Products.OrderBy(p => p.Name), "Id", "Name");
            ViewBag.FieldId = new SelectList(context.Fields.Where(f => f.OwnerId == context.Users
                .Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductId,HarvestDate,Neto,Bruto,ExparationDate,FieldId,Damaged,CircaValue,Urgently")] PersonProduct personProduct)
        {
            //for some reason I need to populate my selectlists again
            ViewBag.ProductId = new SelectList(context.Products.OrderBy(p => p.Name), "Id", "Name");
            ViewBag.FieldId = new SelectList(context.Fields, "Id", "Name");

            personProduct.UserId = context.Users
                .Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id;
            personProduct.Status = true;
            personProduct.Accepted = false;
            personProduct.ProductId = 42;
            if (ModelState.IsValid)
            {
                context.PersonProducts.Add(personProduct);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(personProduct);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonProduct personProduct = context.PersonProducts.Find(id);

            ViewBag.ProductId = new SelectList(context.Products
                .OrderBy(p => p.Id == personProduct.ProductId)
                .OrderBy(p => p.Name), "Id", "Name");

            ViewBag.FieldId = new SelectList(context.Fields.Where(f => f.OwnerId == context.Users
                .Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id), "Id", "Name");

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
        public ActionResult Edit([Bind(Include = "Id,ProductId,HarvestDate,Neto,Bruto,ExparationDate,FieldId,Damaged,CircaValue,Urgently")] PersonProduct personProduct)
        {

            //for some reason I need to populate my selectlists again
            ViewBag.ProductId = new SelectList(context.Products
               .OrderBy(p => p.Id == personProduct.ProductId)
               .OrderBy(p => p.Name), "Id", "Name");

            ViewBag.FieldId = new SelectList(context.Fields.Where(f => f.OwnerId == context.Users
                .Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id), "Id", "Name");

            if (ModelState.IsValid)
            {
                context.Entry(personProduct).State = EntityState.Modified;
                context.SaveChanges();
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
            PersonProduct personProduct = context.PersonProducts.Find(id);
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
            PersonProduct personProduct = context.PersonProducts.Find(id);
            context.PersonProducts.Remove(personProduct);
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
