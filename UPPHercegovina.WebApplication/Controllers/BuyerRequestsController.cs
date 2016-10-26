using PagedList;
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
    [AuthLog(Roles = "Nakupac")]
    public class BuyerRequestsController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private int _pageSize = 5;

        public ActionResult TransactionHistory(int? page)
        {
            var transactions = context.Transactions.OrderByDescending(t => t.Date)
                .Where(t => t.BuyerRequest.BuyerId == context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id)
                .Where(t => t.Status == false && t.Accepted == true)
                .Include(t => t.BuyerRequest.ReservedProducts.Select(r => r.PersonProduct.Product))
                .ToList();

            int pageNumber = (page ?? 1);

            return View(FillModel(transactions).ToPagedList(pageNumber, _pageSize));
        }

        public ActionResult TransactionDetails(int Id, int PersonProductId)
        {
            var transaction = context.Transactions.Where(t => t.Id == Id)
                .Include(t => t.BuyerRequest.ReservedProducts.Select(r => r.PersonProduct.Field))
                .Include(t => t.BuyerRequest.ReservedProducts.Select(r => r.PersonProduct.User))
                .Include(t => t.BuyerRequest.ReservedProducts.Select(r => r.PersonProduct.Product))
                .FirstOrDefault();

            TransactionViewModel ViewModel = new TransactionViewModel();
            ViewModel.Transaction = transaction;
            ViewModel.ReservedProduct = transaction.BuyerRequest.ReservedProducts.Where(r => r.PersonProductId == PersonProductId).FirstOrDefault();
            ViewModel.PersonProduct = ViewModel.ReservedProduct.PersonProduct;

            ViewBag.lat = ViewModel.PersonProduct.Field.GeoLat;
            ViewBag.lng = ViewModel.PersonProduct.Field.GeoLong;

            return View(ViewModel);
        }

        private List<TransactionHistoryViewModel> FillModel(List<Transaction> transactions)
        {
            var ViewModels = new List<TransactionHistoryViewModel>();

            foreach (var transaction in transactions)
            {
                foreach (var item in transaction.BuyerRequest.ReservedProducts)
                {
                    var model = new TransactionHistoryViewModel();
                    model.TransactionId = transaction.Id;
                    model.BuyerRequestId = transaction.BuyerRequestId;
                    model.Date = transaction.Date;
                    model.Price = transaction.Price;
                    model.ReservedProductId = item.Id;
                    model.PersonProductId = item.PersonProductId;
                    model.Product = item.PersonProduct;

                    ViewModels.Add(model);
                }
            }

            return ViewModels;
        }

        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);

            var buyerRequests = context.BuyerRequests
                .Where(b => b.BuyerId == context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id)
                .Include(b => b.Buyer).Where(b => b.Status == true).ToList();

            return View(buyerRequests.ToPagedList(pageNumber, _pageSize));
        }

        public ActionResult RemoveReservedProducts(int? id)
        {
            GlobalData.Instance.ReservedProducts.Remove(GlobalData.Instance.ReservedProducts.Where(p => p.Id == id).FirstOrDefault());
            return RedirectToAction("Create");
        }

        public ActionResult ProductDetails(int? id)
        {
            var personproduct = context.PersonProducts
                 .Where(p => p.Id == id)
                 .Include(p => p.Product)
                 .Include(p => p.User)
                 .Include(p => p.Field)
                 .FirstOrDefault();

            ViewBag.lat = personproduct.Field.GeoLat;
            ViewBag.lng = personproduct.Field.GeoLong;
            return View(personproduct);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuyerRequest buyerRequest = context.BuyerRequests
                .Where(b => b.Id == id)
                .Include(b => b.ReservedProducts
                    .Select(r => r.PersonProduct)
                    .Select(p => p.Product))
                .Include(b => b.ReservedProducts.Select(r => r.PersonProduct).Select(p => p.User))
                .FirstOrDefault();

            if (buyerRequest == null)
            {
                return HttpNotFound();
            }
            return View(buyerRequest);
        }

        public ActionResult Create()
        {
            ViewBag.PersonProducts = GlobalData.Instance.ReservedProducts;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Details,DateOfPickUp")] BuyerRequest buyerRequest)
        {
            buyerRequest.BuyerId = context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id;
            buyerRequest.Status = true;
            buyerRequest.Accepted = false;
            buyerRequest.PickedUp = false;
            buyerRequest.Date = DateTime.Now;

            if (ModelState.IsValid && GlobalData.Instance.ReservedProducts.Count() > 0)
            {
                context.BuyerRequests.Add(buyerRequest);
                context.SaveChanges();

                foreach (var item in GlobalData.Instance.ReservedProducts)
                {
                    var ReservedProduct = new ReservedProduct();
                    ReservedProduct.BuyerRequestId = buyerRequest.Id;
                    ReservedProduct.PersonProductId = item.Id;
                    ReservedProduct.Status = true;
                    ReservedProduct.Canceled = false;
                    ReservedProduct.Details = buyerRequest.Details;

                    context.ReservedProducts.Add(ReservedProduct);
                    context.SaveChanges();

                }
                    //ako zezalo ovdje zeza
                    GlobalData.Instance.ReservedProducts.Clear();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");

        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuyerRequest buyerRequest = context.BuyerRequests.Find(id);
            if (buyerRequest == null)
            {
                return HttpNotFound();
            }
            return View(buyerRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BuyerId,Details,Date,DateOfPickUp,Status,Accepted,PickedUp")] BuyerRequest buyerRequest)
        {
            if (ModelState.IsValid)
            {
                context.Entry(buyerRequest).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(buyerRequest);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuyerRequest buyerRequest = context.BuyerRequests.Find(id);
            if (buyerRequest == null)
            {
                return HttpNotFound();
            }
            return View(buyerRequest);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BuyerRequest buyerRequest = context.BuyerRequests.Find(id);

            var reservedProducts = context.ReservedProducts.Where(r => r.BuyerRequestId == buyerRequest.Id).ToList();

            foreach (var item in reservedProducts)
            {
                context.ReservedProducts.Remove(item);
                context.SaveChanges();
            }

            context.BuyerRequests.Remove(buyerRequest);
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
