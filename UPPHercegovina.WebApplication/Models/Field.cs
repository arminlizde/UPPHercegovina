using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace UPPHercegovina.WebApplication.Models
{
    public class Field
    {
        [DataMember(Name="Id")]
        public int Id { get; set; }

        [Required]
        [DataMember(Name = "Name")]
        [Display(Name="Naziv zemljišta")]
        public string Name { get; set; }

        [Display(Name = "Detalji")]
        [DataMember(Name = "Details")]
        public string Details { get; set; }

        [Display(Name = "Geografska širina")]
        [DataMember(Name = "GeoLong")]
        public string GeoLong { get; set; }

        [DataMember(Name = "GeoLat")]
        [Display(Name = "Geografska dužina")]
        public string GeoLat { get; set; }

        [DataMember(Name = "OwnerId")]
        public string OwnerId { get; set; }

        [DataMember(Name = "Owner")]
        public virtual ApplicationUser Owner { get; set; }
    }
}