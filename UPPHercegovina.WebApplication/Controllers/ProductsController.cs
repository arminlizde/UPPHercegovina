using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UPPHercegovina.WebApplication.CustomFilters;
using UPPHercegovina.WebApplication.Helpers;

namespace UPPHercegovina.WebApplication.Models
{
    [AuthLog(Roles = "Super-Administrator, Administrator")]
    public class ProductsController : Controller
    {
        private int _pageSize = 5;

        private ApplicationDbContext context = new ApplicationDbContext();

        public ActionResult Index(string sortOrder ,int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            ViewBag.NameSortParm = sortOrder == "name" ? "name_desc" : "name";
            ViewBag.TypeSortParm = sortOrder == "type" ? "type_desc" : "type";

            var orderedProducts = context.Products
                .Where(p => p.Status == true).OrderBy(p => p.ProductTypeId).Include(p => p.ProductType).ToList();
            var products = Mapper.MapTo<List<ProductViewModel>, List<Product>>(orderedProducts);
            products = SortFactory.Resolve(sortOrder, products.GetType()).Sort(products);
            
            int pageNumber = (page ?? 1);

            return View(products.ToPagedList(pageNumber, _pageSize));
        }
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = context.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult Create()
        {
            ViewBag.ProductTypeId = new SelectList(context.ProductTypes.Where(p => p.Status == true).
                OrderBy(p => p.Name) , "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "Id,Name,PlaceOfOrigin,Description,PictureUrl,ProductTypeId,Status")] Product product,
           FormCollection form, HttpPostedFileBase file)
        {           
            if (ModelState.IsValid)
            {                          
                product.Status = true;

                if (file != null)
                {
                    if (SavePictureToServer(file, product))
                    {
                        context.Products.Add(product);
                        context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.message = "Please choose only Image file";
                        return RedirectToAction("Index");
                    }
                }

                return View(product);
            }
            return RedirectToAction("Index");
        }

        private bool SavePictureToServer(HttpPostedFileBase file, Product product)
        {
            ProductType type = context.ProductTypes.Find(product.ProductTypeId);

            var path = "/Images/ProductsImg/" + type.Name;

            product.PictureUrl = string.Format("{0}/{1}-{2}.jpg", path, product.Name,
                Extensions.Extensions.GetRndNumber());

            PictureHelper pictureHelper = new PictureHelper(file, product.PictureUrl);

            return pictureHelper.Save();
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //provjeriti jesam li dobio produkte
            Product product = context.Products.Find(id);

            if (product == null)
                return HttpNotFound();

            var productTypesList = context.ProductTypes.
                    OrderByDescending(p => p.Id == product.ProductTypeId).ThenBy(p => p.Name).ToList();

            ViewBag.ProductTypeId = new SelectList(productTypesList, "Id", "Name");
           
            if (String.IsNullOrWhiteSpace(product.PictureUrl))
            {
                product.PictureUrl = "/Images/Hercegovina_grb.png";
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,PlaceOfOrigin,Description,PictureUrl,ProductTypeId,Status")] Product product,
            FormCollection form, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    if (!SavePictureToServer(file, product))
                    {
                        ViewBag.message = "Please choose only Image file";
                        return RedirectToAction("Index");
                    }
                }
                
                context.Entry(product).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }  
            return View(product);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = context.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var product = context.Products.Find(id);
            context.Products.Remove(product);
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
