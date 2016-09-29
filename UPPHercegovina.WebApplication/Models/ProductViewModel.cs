using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using UPPHercegovina.WebApplication.Abstractions;

namespace UPPHercegovina.WebApplication.Models
{
    [DataContractAttribute(Name = "Product")]
    public class ProductViewModel
    {
        [DataMember(Name = "Id")]      
        public int Id { get; set; }

        [Display(Name="Proizvod")]
        [DataMember(Name = "Name")]      
        public string Name { get; set; }

        [Display(Name = "Porijeklo")]
        [DataMember(Name = "PlaceOfOrigin")]      
        public string PlaceOfOrigin { get; set; }

        [Display(Name = "Opis")]
        [DataMember(Name = "Description")]      
        public string Description { get; set; }

        [Display(Name = "Slika")]
        [DataMember(Name = "PictureUrl")]      
        public string PictureUrl { get; set; }

        [Display(Name = "Vrsta")]
        [DataMember(Name = "ProductTypeId")]      
        public string ProductTypeId { get; set; }

        [DataMember(Name = "ProductType")]
        public ProductType ProductType { get; set; }

        public void Set_By_Product(Product p)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                this.Name = p.Name;
                this.Id = p.Id;
                this.PictureUrl = p.PictureUrl;
                this.PlaceOfOrigin = p.PlaceOfOrigin;
                this.Description = p.Description;
                this.ProductTypeId = db.ProductTypes.Find(p.ProductTypeId).Name;
            }
        }
    }
}