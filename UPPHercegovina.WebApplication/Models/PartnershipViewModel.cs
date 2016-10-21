using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UPPHercegovina.WebApplication.Models
{
    public class PartnershipViewModel
    {
        [Display(Name ="Transakcija")]
        public int TransactionId { get; set; }

        [Display(Name = "Kupac")]
        public string BuyerName { get; set; }

        [Display(Name = "Bruto")]
        public string Bruto { get; set; }

        [Display(Name = "Neto")]
        public string Neto { get; set; }

        [Display(Name = "Kvalitet")]
        public string Quality { get; set; }

        [Display(Name = "Vrijednost")]
        public decimal Value { get; set; }

        [Display(Name = "Proizvod")]
        public string ProductName { get; set; }
        
        public DateTime TransactionDate { get; set; }

        public string GetDate { get
            {
                return TransactionDate.Date.ToShortDateString();
            } }

        public int ProductId { get; set; }

        public string BuyerId { get; set; }

        public int BuyerRequestId { get; set; }

        public int PersonProductId { get; set; }

    }

    public class PartnershipViewModelList
    {
        public string BuyerName { get; set; }

        public PersonProduct ProductBestSoldWeight { get; set; }

        public PersonProduct ProductBestSoldPrice { get; set; }

        public PersonProduct ProductBestSoldTransactions { get; set; }


        [Display(Name ="Zarađeno")]
        public decimal Earned { get; set; }

        public decimal Bruto { get; set; }

        public decimal Neto { get; set; }

        [Display(Name = "Prosječna cijena")]
        public decimal AverageValue { get; set; }

        [Display(Name = "Ukupno zarađeno")]
        public decimal SumValue { get; set; }


        public PagedList<PartnershipViewModel> PartnershipsPaged { get; set; }

        public List<PartnershipViewModel> Partnerships { get; set; }
    }
}