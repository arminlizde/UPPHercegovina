namespace UPPHercegovina_PCL.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using UPPHercegovina_WebAPI.Models;
    public partial class AspNetUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AspNetUser()
        {
            //this.Appointments = new HashSet<Appointment>();
            //this.AspNetUserClaims = new HashSet<AspNetUserClaim>();
            //this.AspNetUserLogins = new HashSet<AspNetUserLogin>();
            //this.BuyerRequests = new HashSet<BuyerRequest>();
            //this.Fields = new HashSet<Field>();
            //this.PersonProducts = new HashSet<PersonProduct>();
            //this.Transactions = new HashSet<Transaction>();
            //this.UserMemberships = new HashSet<UserMembership>();
            //this.AspNetRoles = new HashSet<AspNetRole>();
        }

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("EmailConfirmed")]
        public bool EmailConfirmed { get; set; }

        [JsonProperty("PasswordHash")]
        public string PasswordHash { get; set; }

        [JsonProperty("SecurityStamp")]
        public string SecurityStamp { get; set; }

        [JsonProperty("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("PhoneNumberConfirmed")]
        public bool PhoneNumberConfirmed { get; set; }

        [JsonProperty("TwoFactorEnabled")]
        public bool TwoFactorEnabled { get; set; }

        [JsonProperty("LockoutEndDateUtc")]
        public DateTime? LockoutEndDateUtc { get; set; }

        [JsonProperty("LockoutEnabled")]
        public bool LockoutEnabled { get; set; }

        [JsonProperty("AccessFailedCount")]
        public int AccessFailedCount { get; set; }

        [JsonProperty("UserName")]
        public string UserName { get; set; }

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [JsonProperty("IdentificationNumber")]
        public string IdentificationNumber { get; set; }

        [JsonProperty("PictureUrl")]
        public string PictureUrl { get; set; }

        [JsonProperty("TownshipId")]
        public int TownshipId { get; set; }

        [JsonProperty("Address")]
        public string Address { get; set; }

        [JsonProperty("RegistrationDate")]
        public DateTime RegistrationDate { get; set; }

        [JsonProperty("Status")]
        public bool Status { get; set; }

        //[JsonProperty("AspNetRole")]
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<AspNetRole> AspNetRoles { get; set; }

        
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Appointment> Appointments { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        //public virtual Township Township { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<BuyerRequest> BuyerRequests { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Field> Fields { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<PersonProduct> PersonProducts { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Transaction> Transactions { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<UserMembership> UserMemberships { get; set; }
    }
}
