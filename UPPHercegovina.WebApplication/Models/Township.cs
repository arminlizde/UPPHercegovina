using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UPPHercegovina.WebApplication.Models
{
    public class Township
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Obavezno ime općine")]
        public string Name { get; set; }
        //ovdje ne treba required, jer je fiksirano u view
        public string Entity { get; set; }
    }
}