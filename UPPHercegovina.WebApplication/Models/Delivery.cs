using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPPHercegovina.WebApplication.Models
{
    public class Delivery
    {
        public int Id { get; set; }

        public int AppointmentId { get; set; }

        public int PersonProductId { get; set; }

        public virtual PersonProduct PersonProduct { get; set; }

        public virtual Appointment Appointment { get; set; }

        public bool Delivered { get; set; }

        public bool Status { get; set; }
    }
}