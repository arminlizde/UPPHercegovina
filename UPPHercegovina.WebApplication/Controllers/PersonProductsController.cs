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
    public class PersonProductsController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        public ActionResult Index(FormCollection form)
        {
            int SearchIndex = 0;

            if (form.Count > 0)
            {
                var selectListValue = form.GetValue("States");
                SearchIndex = Convert.ToInt32(selectListValue.AttemptedValue);
            }

            List<SelectListItem> StateList = new List<SelectListItem>();
            var item1 = new SelectListItem() { Text = "Aktivni", Value = "1" };
            var item2 = new SelectListItem() { Text = "Prihvaćeni (U obradi)", Value = "2" };
            var item3 = new SelectListItem() { Text = "Odbijeni", Value = "3" };
            StateList.Add(item1);
            StateList.Add(item2);
            StateList.Add(item3);

            ViewBag.States = new SelectList(StateList, "Value", "Text");

            #region SwitchCase
            switch (SearchIndex)
            {
                case 1:
                    return View(context.PersonProducts
                        .Where(p => p.Status == true)
                        .Where(p => p.Accepted == false)
                        .Where(p => p.UserId == context.Users
                          .Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id)
                        .Include(p => p.Field)
                        .Include(p => p.Product).ToList());
                case 2:
                    return View(context.PersonProducts
                       .Where(p => p.Status == true)
                       .Where(p => p.Accepted == true)
                       .Where(p => p.UserId == context.Users
                         .Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id)
                       .Include(p => p.Field)
                       .Include(p => p.Product).ToList());
                case 3:
                    return View(context.PersonProducts
                       .Where(p => p.Status == false)
                       .Where(p => p.Accepted == false)
                       .Where(p => p.UserId == context.Users
                         .Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id)
                       .Include(p => p.Field)
                       .Include(p => p.Product).ToList());
                default: return View(new List<PersonProduct>());
            }
            #endregion

        }

        [AuthLog(Roles = "Super-Administrator, Administrator")]
        public ActionResult Processing()
        {
            return View(context.PersonProducts
                        .Where(p => p.Status == true)
                        .Where(p => p.Accepted == false)
                        .Include(p => p.Warehouse1)
                        .Include(p => p.User)
                        .Include(p => p.Product).ToList());
        }

        [AuthLog(Roles = "Super-Administrator, Administrator")]
        public ActionResult AcceptProcess(int? id)
        {
            ViewBag.Warehouse1Id = new SelectList(context.Warehouses1, "Id", "Name");
            ViewBag.QualityId = new SelectList(context.Qualities, "Name", "Name");
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PersonProduct personProduct = context.PersonProducts.Where(p => p.Id == id)
                .Include(p => p.User)
                .Include(p => p.Field)
                .Include(p => p.Product).FirstOrDefault();

            if (personProduct == null)
            {
                return HttpNotFound();
            }
            return View(personProduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AcceptProcess([Bind(Include = "Id,ProductId,HarvestDate,Neto,Bruto,UserId,Warehouse1Id,ExparationDate,FieldId,Quality,Damaged,CircaValue,Urgently")] PersonProduct personProduct)
        {

            ViewBag.QualityId = new SelectList(context.Qualities, "Name", "Name");


            ViewBag.FieldId = new SelectList(context.Fields.Where(f => f.OwnerId == context.Users
                .Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id), "Id", "Name");

            personProduct.Status = true;
            personProduct.Accepted = true;

            if (ModelState.IsValid)
            {
                context.Entry(personProduct).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(personProduct);
        }

        public ActionResult RejectProcess(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            PersonProduct personProduct = context.PersonProducts
                .Where(p => p.Id == id)
                .Include(p => p.Field)
                .Include(p => p.User)
                .Include(p => p.Product)
                .Include(p => p.Warehouse1)
                .FirstOrDefault();

            if (personProduct == null)
            {
                return HttpNotFound();
            }
            return View(personProduct);
        }

        [HttpPost, ActionName("RejectProcess")]
        [ValidateAntiForgeryToken]
        public ActionResult RejectProcess(int id)
        {
            var personProduct = context.PersonProducts.Find(id);

            personProduct.Accepted = false;
            personProduct.Status = false;

            if (ModelState.IsValid)
            {
                context.Entry(personProduct).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(personProduct);
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PersonProduct personProduct = context.PersonProducts
                .Where(p => p.Id == id)
                .Include(p => p.Field)
                .Include(p => p.User)
                .Include(p => p.Product)
                .Include(p => p.Warehouse1)
                .FirstOrDefault();

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
            //personProduct.Warehouse1Id = null;
           // personProduct.ProductId = 42;
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

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            PersonProduct personProduct = context.PersonProducts
                .Where(p => p.Id == id)
                .Include(p => p.Field)
                .Include(p => p.User)
                .Include(p => p.Product)
                .Include(p => p.Warehouse1)
                .FirstOrDefault();

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
            return RedirectToAction("Processing");
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
