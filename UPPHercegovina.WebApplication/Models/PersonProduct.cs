using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace UPPHercegovina.WebApplication.Models
{
    public class PersonProduct
    {
        [DataMember(Name = "Id")] 
        public int Id { get; set; }

        [Display(Name = "Korisnik")]
        [DataMember(Name = "UserId")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Proizvod")]
        [DataMember(Name = "ProductId")]
        public int ProductId { get; set; }

        [Required]
        [Display(Name = "Datum Berbe")]
        [DataMember(Name = "HarvestDate")]
        public DateTime HarvestDate { get; set; }

        [Display(Name = "Neto količina")]
        [DataMember(Name = "Neto")]
        public double Neto { get; set; }

        [Display(Name = "Bruto količina")]
        [DataMember(Name = "Bruto")]
        public double Bruto { get; set; }

        [Display(Name = "Koordinate porijekla proizvoda")]
        [DataMember(Name = "Coordinates")]
        public string Coordinates { get; set; }

        [Display(Name = "Rok trajanja")]
        [DataMember(Name = "ExparationDate")]
        public DateTime ExparationDate { get; set; }

        [Display(Name = "Vrsta skladištenja")]
        [DataMember(Name = "StoringId")]
        public int StoringId { get; set; }

        [Display(Name = "Kvalitet")]
        [DataMember(Name = "Quality")]
        public string Quality { get; set; }

        [Display(Name = "Skladište")]
        [DataMember(Name = "WarehouseId")]
        public int WarehouseId { get; set; }
    }
}