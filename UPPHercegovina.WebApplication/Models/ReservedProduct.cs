using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UPPHercegovina.WebApplication.Models
{
    public class ReservedProduct
    {
        [Display(Name="Id rezervacije")]
        public int Id { get; set; }

        [Display(Name="Zahtjev")]
        public int BuyerRequestId { get; set; }

        [Display(Name = "Proizvod")]
        public int PersonProductId { get; set; }

        [Display(Name = "Detalji")]
        public string Details { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        [Display(Name = "Odgođeno")]
        public bool Canceled { get; set; }

        public virtual PersonProduct PersonProduct { get; set; }
    }
}