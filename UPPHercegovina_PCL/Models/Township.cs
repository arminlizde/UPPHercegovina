//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UPPHercegovina_PCL.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Township
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Township()
        {
            this.AspNetUsers = new HashSet<AspNetUser>();
            this.Warehouse1 = new HashSet<Warehouse1>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Entity { get; set; }
        public string GeographicPosition_Longitude { get; set; }
        public string GeographicPosition_Latitude { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Warehouse1> Warehouse1 { get; set; }
    }
}
