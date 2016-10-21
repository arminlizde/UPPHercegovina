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

        [Display(Name="Datum kreiranja")]
        public DateTime Date { get; set; }

        [Display(Name = "Datum preuzimanja")]
        public DateTime DateOfPickUp { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        [Display(Name = "Prihvaćeno")]
        public bool Accepted { get; set; }

        [Display(Name ="Pokupljeno")]
        public bool PickedUp { get; set; }

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

        [Display(Name = "Ukupna cijena")]
        public decimal ValueSum { get
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var products = context.ReservedProducts
                        .Where(rp => rp.BuyerRequestId == Id).Select(rp => rp.PersonProduct)
                        .ToList();

                    decimal sum = 0;

                    foreach (var item in products)
                    {
                        sum += item.Value * Convert.ToDecimal(item.Neto);
                    }

                    return sum;
                }

            } }
    }
}