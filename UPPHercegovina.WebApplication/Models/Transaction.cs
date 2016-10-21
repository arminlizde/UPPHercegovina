using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UPPHercegovina.WebApplication.Models
{
    public class Transaction
    {
        [Display(Name = "Transakcija")]
        public int Id { get; set; }

        [Display(Name = "Zahtjev")]
        public int BuyerRequestId { get; set; }

        [Display(Name = "Administrator")]
        public string UserId { get; set; } 

        [Display(Name = "Detalji")]
        public string Details { get; set; }

        [Display(Name = "Cijena")]
        public decimal Price { get; set; }

        [Display(Name = "Datum")]
        public DateTime Date { get; set; }

        [Display(Name = "Datum")]
        public string GetOnlyDate { get { return Date.ToShortDateString(); } }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        [Display(Name = "Prihvaćeno")]
        public bool Accepted { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual BuyerRequest BuyerRequest { get; set; }

    }

    public class TransactionViewModel
    {
        public Transaction Transaction { get; set; }

        public ReservedProduct ReservedProduct { get; set; }

        public string BuyerFullName { get; set; }
    }
}