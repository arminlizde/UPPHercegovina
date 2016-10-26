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
    [AuthLog(Roles = "Super-Administrator, Administrator")]
    public class TransactionsController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private int _pageSize = 10;

        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);

            return View(context.Transactions.OrderByDescending(t => t.Date).ThenBy(t => t.Id)
                .Where(t => t.Status == false)
                .Where(t => t.Accepted == true)
                .Include(t => t.User).ToPagedList(pageNumber, _pageSize));
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //namireno ako trebalo dodavati vise informacija
            Transaction transaction = context.Transactions
                .Where(t => t.Id == id)
                .Include(t => t.User)
                .Include(t => t.BuyerRequest.ReservedProducts.Select(p => p.PersonProduct)).FirstOrDefault();
                

            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        public ActionResult FinishTransaction(int? id, bool status)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var transaction = context.BuyerRequests.Find(id);

            if (transaction == null)
            {
                return HttpNotFound();
            }

            transaction.Accepted = status;
            context.Entry(transaction).State = EntityState.Modified;
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
