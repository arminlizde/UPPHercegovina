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
        public ApplicationDbContext context = new ApplicationDbContext();

        public ActionResult ActiveRequests()
        {
            return View(context.BuyerRequests
                .Where(r => r.Status == true)
                .Where(r => r.Accepted == false)
                .Include(r => r.Buyer));
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