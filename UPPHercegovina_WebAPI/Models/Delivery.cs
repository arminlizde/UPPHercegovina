//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UPPHercegovina_WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Delivery
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int PersonProductId { get; set; }
        public bool Delivered { get; set; }
        public bool Status { get; set; }
    
        public virtual Appointment Appointment { get; set; }
        public virtual PersonProduct PersonProduct { get; set; }
    }
}