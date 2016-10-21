using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Controllers
{
    public class PartnershipsController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        private int _pageSize = 5;

        public ActionResult TransactionHistory(int? page)
        {
            int pageNumber = (page ?? 1);

            return View(FillPartnershipViewModel().ToPagedList(pageNumber, _pageSize));
        }

        public ActionResult Details(int? id, int? transactionId)
        {
            var Transaction = context.Transactions.Where(t => t.Id == transactionId).OrderByDescending(tran => tran.Id)
                .Include(T => T.BuyerRequest)
                .Include(BR => BR.BuyerRequest.ReservedProducts.Select(rp => rp.PersonProduct)
                .Select(pro => pro.Product))
                .Include(pp => pp.User)
                .FirstOrDefault();

            var TransactionViewModel = new TransactionViewModel();
            TransactionViewModel.Transaction = Transaction;
            TransactionViewModel.BuyerFullName = context.Users.Find(Transaction.BuyerRequest.BuyerId).FullName;

            foreach (var item in Transaction.BuyerRequest.ReservedProducts)
            {
                if(item.PersonProductId == id)
                {
                    TransactionViewModel.ReservedProduct = item;
                    break;
                }
            }

            return View(TransactionViewModel);
        }

        public ActionResult BuyerDetails(string id)
        {
            PartnershipViewModelList Partnership = new PartnershipViewModelList();
            var list = FillPartnershipViewModel();
            Partnership.Partnerships = new List<PartnershipViewModel>();
            Partnership.Bruto = 0;
            Partnership.Neto = 0;
            Partnership.Earned = 0;
            Partnership.SumValue = 0;

            Partnership.BuyerName = context.Users.Find(id).FullName;


            foreach (var item in list)
            {
                if(item.BuyerId == id)
                    Partnership.Partnerships.Add(item);
            }

            Partnership.ProductBestSoldTransactions = new PersonProduct();
            var most = (from i in list
                        group i by i into grp
                        orderby grp.Count() descending
                        select grp.Key).First();

            Partnership.ProductBestSoldTransactions = context.PersonProducts.Find(most.PersonProductId);

            foreach (var item in Partnership.Partnerships)
            {

                Partnership.SumValue += item.Value * Convert.ToDecimal(item.Neto);

                if(item.ProductId == most.ProductId)
                {
                    Partnership.Bruto += Convert.ToDecimal(item.Bruto);
                    Partnership.Neto += Convert.ToDecimal(item.Neto);
                    Partnership.AverageValue += item.Value;
                }
            }

            Partnership.AverageValue = Math.Round(Partnership.AverageValue / Partnership.Partnerships.Count(),2);

            Partnership.Earned = Partnership.Neto * Partnership.AverageValue;

            return View(Partnership);
        }


        private List<PartnershipViewModel> FillPartnershipViewModel()
        {
            string UserId = context.Users
                .Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id;

            var Transactions = context.Transactions.OrderByDescending(tran => tran.Id)
                .Include(T => T.BuyerRequest)
                .Include(BR => BR.BuyerRequest.ReservedProducts.Select(rp => rp.PersonProduct).Select(pro => pro.Product))
                .Include(pp => pp.User)
                .ToList();

            List<PartnershipViewModel> list = new List<PartnershipViewModel>();

            foreach (var transaction in Transactions)
            {

                foreach (var item in transaction.BuyerRequest.ReservedProducts)
                {
                    if (item.PersonProduct.UserId == UserId)
                    {
                        PartnershipViewModel temp = new PartnershipViewModel();

                        temp.TransactionId = transaction.Id;
                        temp.BuyerName = context.Users.Find(transaction.BuyerRequest.BuyerId).FullName;
                        temp.BuyerRequestId = transaction.BuyerRequestId;
                        temp.Bruto = item.PersonProduct.Bruto;
                        temp.Neto = item.PersonProduct.Neto;
                        temp.Quality = item.PersonProduct.Quality;
                        temp.Value = item.PersonProduct.Value;
                        temp.ProductName = item.PersonProduct.Product.Name;
                        temp.ProductId = item.PersonProduct.Product.Id;
                        temp.TransactionDate = transaction.Date;
                        temp.PersonProductId = item.PersonProductId;
                        temp.BuyerId = transaction.BuyerRequest.BuyerId;

                        list.Add(temp);
                    }
                }
            }

            return list;
        }
    }
}