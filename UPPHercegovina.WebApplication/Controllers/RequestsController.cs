using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using UPPHercegovina.WebApplication.CustomFilters;
using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Controllers
{

    [AuthLog(Roles = "Super-Administrator, Administrator")]
    public class RequestsController : Controller
    {
        private int _pageSize = 5;
        public ApplicationDbContext context = new ApplicationDbContext();

        public ActionResult ActiveRequests(int? page)
        {
            int pageNumber = (page ?? 1);

            return View(context.BuyerRequests.OrderByDescending(r => r.DateOfPickUp).ThenBy(r => r.Date)
                .Where(r => r.Status == true)
                .Where(r => r.Accepted == false)
                .Include(r => r.Buyer).ToPagedList(pageNumber, _pageSize));
        }

        public ActionResult History(int? page)
        {
            int pageNumber = (page ?? 1);
            var buyerRequests = context.BuyerRequests.OrderBy(b => b.Accepted).ThenByDescending(b => b.DateOfPickUp)
                .Include(b => b.Buyer).Where(b => b.Status == false);
            return View(buyerRequests.ToPagedList(pageNumber, _pageSize));
        }

        public ActionResult AcceptedRequests(int? page)
        {
            int pageNumber = (page ?? 1);

            return View(context.BuyerRequests.OrderByDescending(r => r.DateOfPickUp).ThenBy(r => r.Date)
                .Where(r => r.Status == true)
                .Where(r => r.Accepted == true)
                .Include(r => r.Buyer).ToPagedList(pageNumber, _pageSize));
        }
    
        public ActionResult PickUp(int? id)
        {
            var request = context.BuyerRequests.Where(b => b.Id == id)
                .Include(b => b.ReservedProducts.Select(r => r.PersonProduct)).FirstOrDefault();

            if(request != null)
            {
                request.Status = false;
                request.PickedUp = true;

                context.Entry(request).State = EntityState.Modified;
                context.SaveChanges();

                var transaction = context.Transactions.Where(t => t.BuyerRequestId == request.Id).FirstOrDefault();
                transaction.Status = false;
                transaction.Accepted = true;

                context.Entry(transaction).State = EntityState.Modified;
                context.SaveChanges();

                foreach (var reservedProduct in request.ReservedProducts)
                {
                    reservedProduct.Status = false;
                    context.Entry(reservedProduct).State = EntityState.Modified;
                    context.SaveChanges();

                    reservedProduct.PersonProduct.Status = false;
                    reservedProduct.PersonProduct.InWarehouse = false;

                    context.Entry(reservedProduct.PersonProduct).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }

            return RedirectToAction("AcceptedRequests");
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request = context.BuyerRequests.Where(r => r.Id == id)
                .Include(r => r.ReservedProducts.Select(rp => rp.PersonProduct).Select(p => p.Product))
                .Include(r => r.ReservedProducts.Select(rp => rp.PersonProduct).Select(pp => pp.Warehouse1))
                .Include(r => r.Buyer).FirstOrDefault();

            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        public ActionResult ProcessRequest(int? id, bool status)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request = context.BuyerRequests.Find(id);

            if (request == null)
            {
                return HttpNotFound();
            }

            if(status)
            {
                var transaction = new Transaction();
                transaction.Status = true;
                transaction.Accepted = false;
                transaction.BuyerRequestId = Convert.ToInt32(id);
                transaction.Price = request.ValueSum;
                transaction.UserId = context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id;
                transaction.Date = DateTime.Now;

                context.Transactions.Add(transaction);
                context.SaveChanges();
            }

            request.Accepted = status;
            context.Entry(request).State = EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("ActiveRequests");          
        }
    }
}