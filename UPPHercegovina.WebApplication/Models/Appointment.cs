using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UPPHercegovina.WebApplication.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        [Display(Name ="Detalji")]
        public string Details { get; set; }

        [Required]
        [Display(Name = "Datum dostavljanja")]
        [DataType(DataType.Date)]
        public DateTime DeliveryDate { get; set; }

        [Display(Name = "Korisnik")]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Display(Name = "Dostavljeno")]
        public bool Delivered { get; set; }

        [Display(Name ="Otkazano")]
        public bool Canceled { get; set; }

        [Display(Name ="Status")]
        public bool Status { get; set; }

        public virtual PersonProduct PersonProduct { get; set; }

        public virtual List<Delivery> Deliveries { get; set; }
    }
}