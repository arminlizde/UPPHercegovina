using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace UPPHercegovina.WebApplication.Models
{

    [DataContractAttribute(Name = "Warehouse, Township")]

    public class GeographicPosition
    {
        [DataMember(Name = "Longitude")]
        [Display(Name = "Geografska dužina")]
        public string Longitude { get; set; }

        [DataMember(Name = "Latitude")]
        [Display(Name = "Geografska širina")]
        public string Latitude { get; set; }
    }
}