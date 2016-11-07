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
    
    public partial class BuyerRequest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BuyerRequest()
        {
            this.ReservedProducts = new HashSet<ReservedProduct>();
            this.Transactions = new HashSet<Transaction>();
        }
    
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public string Details { get; set; }
        public System.DateTime Date { get; set; }
        public bool Status { get; set; }
        public bool Accepted { get; set; }
        public System.DateTime DateOfPickUp { get; set; }
        public bool PickedUp { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReservedProduct> ReservedProducts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}