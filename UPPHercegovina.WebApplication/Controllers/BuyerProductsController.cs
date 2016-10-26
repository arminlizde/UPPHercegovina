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
    public class BuyerProductsController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        private int _pageSize = 5;

        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);

            var list = context.PersonProducts.OrderByDescending(p => p.HarvestDate).ThenBy(p => p.UserId)
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

            return View(viewList.ToPagedList(pageNumber, _pageSize));
        }

        public ActionResult ReservedProducts(int? page)
        {
            int pageNumber = (page ?? 1);

            return View(GlobalData.Instance.ReservedProducts.ToPagedList(pageNumber, _pageSize));
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
            var personproduct = context.PersonProducts
                .Where(p => p.Id == id)
                .Include(p => p.Product)
                .Include(p => p.User)
                .Include(p => p.Field)
                .Include(p => p.Warehouse1)
                .FirstOrDefault();

            ViewBag.lat = personproduct.Field.GeoLat;
            ViewBag.lng = personproduct.Field.GeoLong;
            ViewBag.lat2 = personproduct.Warehouse1.GeographicPosition.Latitude;
            ViewBag.lng2 = personproduct.Warehouse1.GeographicPosition.Longitude;

            return View(personproduct);
        }

        public ActionResult GradeProducts(int? page)
        {
            string buyerId = context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id;

            var requests = context.BuyerRequests.Where(r => r.BuyerId == buyerId)
                .Include(r => r.ReservedProducts.Select(p => p.PersonProduct.Product))
                .Include(r => r.ReservedProducts.Select(p => p.PersonProduct.User))
                .Include(r => r.ReservedProducts.Select(p => p.PersonProduct.Warehouse1)).ToList();

            var personproducts = new List<PersonProduct>();

            foreach (var buyerRequest in requests)
            {
                foreach (var item in buyerRequest.ReservedProducts)
                {
                    if(item.PersonProduct.Rating == 0)
                    {
                        personproducts.Add(item.PersonProduct);
                    }
                }

            }

            int pageNumber = (page ?? 1);

            return View(personproducts.ToPagedList(pageNumber, _pageSize));
        }

        public ActionResult GradeProduct(int id)
        {
            var product = context.PersonProducts.Where(p => p.Id == id)
                .Include(p => p.Product)
                .Include(p => p.Warehouse1)
                .Include(p => p.Field)
                .Include(p => p.User).FirstOrDefault();

            ViewBag.lat = product.Field.GeoLat;
            ViewBag.lng = product.Field.GeoLong;


            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GradeProduct([Bind(Include = "Id,ProductId,HarvestDate,Neto,Bruto,ExparationDate,FieldId,Damaged,CircaValue,Value,Urgently,Status,UserId,Rating")] PersonProduct personProduct)
        {
            if (ModelState.IsValid)
            {
                context.Entry(personProduct).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("GradeProducts");
            }
            return View(personProduct);
        }

        public ActionResult ProducerDetails(string id)
        {
            var user = context.Users.Where(u => u.Id == id).Include(u => u.Township).FirstOrDefault();
            return user != null ? View(user) : View(new ApplicationUser());
        }
    }
}