using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace UPPHercegovina.WebApplication.Models
{
    [DataContractAttribute(Name = "Product")]
    public class Product
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage="Obavezno ime produkta")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Brojevi i znakovi nisu dozvoljeni!")]
        [Display(Name = "Proizvod")]
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
        public int ProductTypeId { get; set; }

        [DataMember(Name = "Status")]
        public bool Status { get; set; }

        [DataMember(Name = "ProductType")]
        public virtual ProductType ProductType { get; set; }

    }
}