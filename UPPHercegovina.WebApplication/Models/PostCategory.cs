using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UPPHercegovina.WebApplication.Models
{
    public class PostCategory
    {
        public int Id { get; set; }

        [Required]
        [Display(Name ="Naziv kategorije")]
        public string Title { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }
    }
}