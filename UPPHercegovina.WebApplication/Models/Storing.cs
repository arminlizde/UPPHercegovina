using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace UPPHercegovina.WebApplication.Models
{
    [DataContractAttribute(Name = "Storing")]
    public class Storing
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Naziv skladištenja")]
        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Opis")]
        [DataMember(Name = "Description")]
        public string Description { get; set; }
    }
}