using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Controllers
{
    public class WarehouseProductsController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        private int _pageSize = 5;

        public ActionResult Index(int? page,FormCollection form)
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
            var UserId = context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id;

            if(SearchIndex != 0)
                PersonProducts = context.PersonProducts.Include(p => p.Product)
                .Where(p => p.UserId == UserId)
                .Where(p => p.InWarehouse == true)
                .Where(p => p.Warehouse1Id == SearchIndex)
                .OrderByDescending(p => p.HarvestDate).ToList();
            else
                 PersonProducts = context.PersonProducts.Include(p => p.Product)
                .Where(p => p.UserId == UserId)
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

            if (personProduct == null)
            {
                return HttpNotFound();
            }
            return View(personProduct);
        }
    }
}