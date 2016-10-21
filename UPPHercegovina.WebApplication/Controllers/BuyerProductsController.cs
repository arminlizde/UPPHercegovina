using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Controllers
{
    public class BuyerProductsController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public ActionResult Index()
        {
            var list = context.PersonProducts
                .Where(p => p.InWarehouse == true)
                .Include(p => p.Field)
                .Include(p => p.User)
                .Include(p => p.Warehouse1)
                .Include(p => p.Product).ToList();

            var viewList = new List<PersonProduct>();

            foreach (var item in list)
            {
                if (GlobalData.Instance.ReservedProducts.Any(x => x.Id == item.Id))
                    continue;
                else
                    viewList.Add(item);
            }

            return View(viewList);
        }

        public ActionResult ReservedProducts()
        {
            return View(GlobalData.Instance.ReservedProducts);
        }

        public ActionResult RemoveReservedProducts(int? id)
        {
            GlobalData.Instance.ReservedProducts.Remove(GlobalData.Instance.ReservedProducts.Where(p => p.Id == id).FirstOrDefault());
            return RedirectToAction("ReservedProducts");
        }

        public ActionResult ReserveProduct(int? id)
        {
            GlobalData.Instance.ReservedProducts.Add(context.PersonProducts
                .Where(p => p.Id == id).Include(p => p.User)
                .Include(p => p.Product)
                .Include(p => p.Warehouse1).FirstOrDefault());
            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
           return View(context.PersonProducts
                .Where(p => p.Id == id)
                .Include(p => p.Product)
                .Include(p => p.User)
                .FirstOrDefault());
        }
    }
}