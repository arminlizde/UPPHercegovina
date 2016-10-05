using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UPPHercegovina.WebApplication.CustomFilters;

using UPPHercegovina.WebApplication.Helpers;
using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Controllers
{

    [AuthLog(Roles = "Super-Administrator, Administrator")]
    public class WarehousesController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        public ActionResult Index()
        {
            var warehouses = context.Warehouses1
                .Include(w => w.Storing)
                .Include(t => t.Township)
                .ToList();
            var vare = Mapper.MapTo<List<WarehouseViewModel>, List<Warehouse1>>(warehouses);

            return View(vare);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Warehouse1 warehouse = context.Warehouses1.Find(id);
            if (warehouse == null)
            {
                return HttpNotFound();
            }
            return View(warehouse);
        }

        public ActionResult Create()
        {
            var storingList = GetStoringSelectList();
            var townshipList = GetTownshipSelectList();     
            ViewBag.StoringId = new SelectList(storingList, "StoringId", "Name");
            ViewBag.TownshipId = new SelectList(townshipList, "TownshipId", "Name");

            return View();
        }

        public List<WarehouseViewModel> GetStoringSelectList()
        {
            var storingList = context.Storings.ToList();
            var warehouseViewModels = new List<WarehouseViewModel>();

            storingList.ForEach(item =>
            {
                var warehouseviewmodel = new WarehouseViewModel() {
                    StoringId = item.Id,
                    Name = item.Name
                };

                warehouseViewModels.Add(warehouseviewmodel);
            });

            return warehouseViewModels;
        }
        public List<TownshipViewModel> GetTownshipSelectList()
        {
            var townshipList = context.PlaceOfResidences.ToList();
            var townshipViewList = new List<TownshipViewModel>();

            townshipList.ForEach(item =>
            {
                var township = new TownshipViewModel()
                {
                    TownshipId = item.Id,
                    Name = item.Name
                };

                townshipViewList.Add(township);
            });
            return townshipViewList;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,StoringId,Location,TownshipId,Status")] WarehouseViewModel warehouseviewmodel,
            FormCollection form)
        {
            var warehouse = Mapper.MapTo<Warehouse1, WarehouseViewModel>(warehouseviewmodel);

            if (ModelState.IsValid)
            {
                context.Warehouses1.Add(warehouse);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(warehouse);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Warehouse1 warehouse = context.Warehouses1.Find(id);
            var storings = context.Storings.
                    OrderByDescending(s => s.Id == warehouse.StoringId).ThenBy(p => p.Name).ToList();
            var townships = context.PlaceOfResidences
                .OrderByDescending(t => t.Id == warehouse.TownshipId)
                .ThenBy(t => t.Name).ToList();

            ViewBag.TownshipId = new SelectList(townships, "Id", "Name");
            ViewBag.StoringId = new SelectList(storings, "Id", "Name");

            if (warehouse == null)
            {
                return HttpNotFound();
            }
            return View(warehouse);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StoringId,TownshipId,Location,Status")] Warehouse1 warehouse)
        {
            if (ModelState.IsValid)
            {
                context.Entry(warehouse).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(warehouse);
        }

        #region nijePotrebno
        // GET: Warehouses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Warehouse1 warehouse = context.Warehouses1.Find(id);
            if (warehouse == null)
            {
                return HttpNotFound();
            }
            return View(warehouse);
        }

        // POST: Warehouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Warehouse warehouse = context.Warehouses.Find(id);
            context.Warehouses.Remove(warehouse);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion

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
