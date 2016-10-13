using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Controllers
{
    public class PartnershipsController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public ActionResult TransactionHistory()
        {
            string UserId = context.Users
                .Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id;

            var list = context.Transactions.Include(t => t.BuyerRequest)
                .Select(b => b.BuyerRequest.ReservedProducts
                    .Select(br => br.PersonProduct)
                    .Select(pp => pp.Field)).ToList();

                
            return View();
        }
    }
}