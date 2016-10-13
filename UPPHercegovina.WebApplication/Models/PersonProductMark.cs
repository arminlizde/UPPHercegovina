using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPPHercegovina.WebApplication.Models
{
    public class PersonProductMark
    {
        public int Id { get; set; }

        public int PersonProductId { get; set; }

        public int MarkId { get; set; }

        public virtual PersonProduct PersonProduct { get; set; }

        public virtual Mark Mark { get; set; }
    }
}