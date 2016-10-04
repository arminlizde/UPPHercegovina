using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace UPPHercegovina.WebApplication.Models
{
    [DataContractAttribute(Name = "Post")]
    public class Post
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name="Naslov")]
        [DataMember(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Text")]
        [DataMember(Name = "Text")]
        public string Text { get; set; }

        [Required]
        [Display(Name = "Autor")]
        [DataMember(Name = "Author")]
        public string Author { get; set; }

        [Display(Name = "Vrijeme objave")]
        [DataMember(Name = "PostDate")]
        public DateTime PostDate { get; set; }

        [Display(Name = "Slika")]
        [DataMember(Name = "PictureUrl")]        
        public string PictureUrl { get; set; }

        [Display(Name = "Preporučeno")]
        [DataMember(Name = "Recommended")]
        public bool Recommended { get; set; }

        [Display(Name = "Status")]
        [DataMember(Name = "Status")]
        public bool Status { get; set; }

        [DataMember(Name = "CategoryId")]
        public int CategoryId { get; set; }
    }
}