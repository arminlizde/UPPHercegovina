using System;
using System.Collections.Generic;
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
    public class ProcessesController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        public ActionResult Processing()
        {
            return View(context.PersonProducts
                        .Where(p => p.Status == true)
                        .Where(p => p.Accepted == false)
                        .Include(p => p.Warehouse1)
                        .Include(p => p.User)
                        .Include(p => p.Product).ToList());
        }

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
                
            }
            //return View(personProduct);
            return RedirectToAction("Processing");
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
            }
            return RedirectToAction("Processing");

            //return View(personProduct);
        }
    }
}