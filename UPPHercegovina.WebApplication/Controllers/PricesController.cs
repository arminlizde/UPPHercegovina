using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UPPHercegovina.WebApplication.CustomFilters;
using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Controllers
{
    [AuthLog(Roles = "Nakupac")]

    public class PricesController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private int _pageSize = 5;

        public ActionResult PriceList(int? page)
        {
            var products = context.PersonProducts.OrderBy(p => p.ProductId)
                .Where(p => p.InWarehouse == true)
                .Include(p => p.Product)
                .ToList();

            int pageNumber = (page ?? 1);

            return View(products.ToPagedList(pageNumber, _pageSize));
        }
    }
}