namespace UPPHercegovina_WebAPI.Models
{
    using Newtonsoft.Json;

    public partial class AspNetRole
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AspNetRole()
        {
            //this.AspNetUsers = new HashSet<AspNetUser>();
        }
    
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }
    
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
    }
}
