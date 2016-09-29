using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UPPHercegovina.WebApplication.Models
{
    public class Membership
    {
        public int Id { get; set; }

        [Required]
        [Display(Name="Naziv")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Opis")]
        public string Description { get; set; }
        
        [Required]
        [Display(Name = "Cijena")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "Datum kreiranja")]
        public DateTime CreationDate { get; set; }

        public bool Status { get; set; }
    }
}