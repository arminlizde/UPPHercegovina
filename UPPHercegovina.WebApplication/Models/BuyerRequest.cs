using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UPPHercegovina.WebApplication.Models
{
    public class BuyerRequest
    {
        public int Id { get; set; }

        [Display(Name = "Kupac")]
        public string BuyerId { get; set; }

        [Display(Name = "Detalji")]
        public string Details { get; set; }

        [Display(Name="Datum")]
        public DateTime Date { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        [Display(Name = "Prihvaćeno")]
        public bool Accepted { get; set; }

        public virtual List<ReservedProduct> ReservedProducts { get; set; }

        public virtual ApplicationUser Buyer { get; set; }

        [Display(Name = "Broj proizvoda")]
        public int ProductCount { get
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    return context.ReservedProducts.Where(rp => rp.BuyerRequestId == Id).Count();
                }
            } }
    }
}