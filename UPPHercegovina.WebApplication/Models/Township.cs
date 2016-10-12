using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace UPPHercegovina.WebApplication.Models
{
    public class Township
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Obavezno ime općine")]
        public string Name { get; set; }
        //ovdje ne treba required, jer je fiksirano u view
        [Display(Name="Entitet")]
        public string Entity { get; set; }

        [DataMember(Name= "GeographicPosition")]
        public GeographicPosition GeographicPosition { get; set; }
    }

    public class TownshipViewModel
    {
        [Display(Name="Općina")]
        public int TownshipId { get; set; }
        public string Name { get; set; }
        [DataMember(Name = "GeographicPosition")]
        public GeographicPosition GeographicPosition { get; set; }
    }
}