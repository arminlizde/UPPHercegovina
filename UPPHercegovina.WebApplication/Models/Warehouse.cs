using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace UPPHercegovina.WebApplication.Models
{
    //Ova mi klasa vise ne treba
    [DataContractAttribute(Name = "Warehouse")]
    public class Warehouse
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [DataMember(Name="Name")]
        [Display(Name="Skladište")]
        public string Name { get; set; }

        [Display(Name="Opis")]
        [DataMember(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Vrsta skladištenja")]
        [DataMember(Name = "StoringId")]
        public string StoringId { get; set; }

        [DataMember(Name = "Location")]
        public string Location { get; set; }

        [DataMember(Name = "Storing")]
        public virtual Storing Storing { get; set; }
    }

    [DataContractAttribute(Name = "Warehouse")]
    public class Warehouse1
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [DataMember(Name = "Name")]
        [Display(Name = "Skladište")]
        public string Name { get; set; }

        [Display(Name = "Opis")]
        [DataMember(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Vrsta skladištenja")]
        [DataMember(Name = "StoringId")]
        public int? StoringId { get; set; }

        [DataMember(Name = "Storing")]
        public virtual Storing Storing { get; set; }

        [Display(Name = "Općina")]
        [DataMember(Name = "TownshipId")]
        public int? TownshipId { get; set; }

        [DataMember(Name = "Township")]
        public virtual Township Township { get; set; }

        [DataMember(Name ="Status")]
        public bool Status { get; set; }

        [DataMember(Name = "GeographicPosition")]
        public GeographicPosition GeographicPosition { get; set; }
    }

    [DataContractAttribute(Name = "Warehouse")]
    public class WarehouseViewModel
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "StoringId")]
        [Display(Name="Vrsta skladištenja")]
        public int StoringId { get; set; }

        [Required]
        [DataMember(Name = "Name")]
        [Display(Name = "Skladište")]
        public string Name { get; set; }

        [Display(Name = "Opis")]
        [DataMember(Name = "Description")]
        public string Description { get; set; }

        [DataMember(Name = "Storing")]
        public Storing Storing { get; set; }

        [DataMember(Name = "TownshipId")]
        [Display(Name = "Općina")]
        public int? TownshipId { get; set; }

        [DataMember(Name = "Township")]
        public Township Township { get; set; }

        [DataMember(Name = "GeographicPosition")]
        public GeographicPosition GeographicPosition { get; set; }

        [Display(Name = "Status")]
        [DataMember(Name = "Status")]
        public bool Status { get; set; }
    }
}