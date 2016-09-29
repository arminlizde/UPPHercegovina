using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace UPPHercegovina.WebApplication.Models
{
    [DataContractAttribute(Name = "ProductType")]
    public class ProductType
    {
        public int Id { get; set; }

        [Required(ErrorMessage="Obavezno ime vrste proizvoda")]
        [RegularExpression(@"^[a-zA-ZšćčđžŠČĆŽĐ\s]*$", ErrorMessage = "Brojevi i znakovi nisu dozvoljeni!")]
        [Display(Name = "Naziv")]
        [DataMember(Name = "Type")]
        public string Name { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }
        
        public bool Status { get; set; }
    }
}