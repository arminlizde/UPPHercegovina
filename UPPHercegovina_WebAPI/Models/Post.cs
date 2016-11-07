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
    
    public partial class Post
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Post()
        {
            this.Posts1 = new HashSet<Post>();
            this.Posts11 = new HashSet<Post>();
            this.Posts12 = new HashSet<Post>();
        }
    
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public System.DateTime PostDate { get; set; }
        public string PictureUrl { get; set; }
        public bool Recommended { get; set; }
        public bool Status { get; set; }
        public int CategoryId { get; set; }
        public Nullable<int> Post_Id { get; set; }
        public Nullable<int> Post_Id1 { get; set; }
        public Nullable<int> Post_Id2 { get; set; }
        public byte[] Picture { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Post> Posts1 { get; set; }
        public virtual Post Post1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Post> Posts11 { get; set; }
        public virtual Post Post2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Post> Posts12 { get; set; }
        public virtual Post Post3 { get; set; }
    }
}