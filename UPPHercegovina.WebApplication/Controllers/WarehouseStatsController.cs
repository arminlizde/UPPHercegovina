using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using UPPHercegovina.WebApplication.CustomFilters;
using UPPHercegovina.WebApplication.Models;
using PagedList;
using System.Net;

namespace UPPHercegovina.WebApplication.Controllers
{
    [AuthLog(Roles = "Super-Administrator, Administrator")]
    public class WarehouseStatsController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        private int _pageSize = 5;

        public ActionResult Index(int? page, FormCollection form)
        {
            int SearchIndex = 0;

            if (form.Count > 0)
            {
                var selectListValue = form.GetValue("States");
                SearchIndex = Convert.ToInt32(selectListValue.AttemptedValue);
            }

            var warehouse = new Warehouse1();
            warehouse.Id = 0;
            warehouse.Name = "Sva skladišta";

            var warehouses = context.Warehouses1
                .Where(w => w.Status == true).OrderBy(w => w.Id).OrderBy(w => w.TownshipId).ToList();

            warehouses.Insert(0, warehouse);

            var StateList = new SelectList(warehouses, "Id", "Name");
            ViewBag.States = new SelectList(StateList, "Value", "Text");

            var PersonProducts = new List<PersonProduct>();

            if (SearchIndex != 0)
                PersonProducts = context.PersonProducts.Include(p => p.Product)
                .Where(p => p.InWarehouse == true)
                .Where(p => p.Warehouse1Id == SearchIndex)
                .OrderByDescending(p => p.HarvestDate).ToList();
            else
                PersonProducts = context.PersonProducts.Include(p => p.Product)
               .Where(p => p.InWarehouse == true)
               .OrderBy(p => p.Warehouse1Id)
               .ThenByDescending(p => p.HarvestDate).ToList();

            int pageNumber = (page ?? 1);

            return View(PersonProducts.ToPagedList(pageNumber, _pageSize));
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

            ViewBag.lat = personProduct.Field.GeoLat;
            ViewBag.lng = personProduct.Field.GeoLong;

            if (personProduct == null)
            {
                return HttpNotFound();
            }
            return View(personProduct);
        }

        public ActionResult RemoveFromWarehouse(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = context.PersonProducts.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            product.InWarehouse = false;
            product.Accepted = false;

            context.Entry(product).State = EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
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
        public ActionResult Edit([Bind(Include = "Id,ProductId,HarvestDate,InWarehouse,Neto,Bruto,UserId,Warehouse1Id,ExparationDate,FieldId,Status,Accepted,Quality,Damaged,CircaValue,Urgently,Value")] PersonProduct personProduct)
        {

            ViewBag.QualityId = new SelectList(context.Qualities, "Name", "Name");


            ViewBag.FieldId = new SelectList(context.Fields.Where(f => f.OwnerId == context.Users
                .Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id), "Id", "Name");

            if (ModelState.IsValid)
            {
                context.Entry(personProduct).State = EntityState.Modified;
                context.SaveChanges();

            }
            //return View(personProduct);
            return RedirectToAction("Index");
        }
    }
}