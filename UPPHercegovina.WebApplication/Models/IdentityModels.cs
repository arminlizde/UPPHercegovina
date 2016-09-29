using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System; //za datetime

namespace UPPHercegovina.WebApplication.Models
{

    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public string IdentificationNumber { get; set; }
        public string PictureUrl { get; set; }
        public int TownshipId { get; set; }
        public string Address { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool Status { get; set; }

        public string GetDisplayName()
        {
            var displayName = String.Format("{0} {1}", this.FirstName, this.LastName);

            return !String.IsNullOrWhiteSpace(displayName) ? displayName : this.Email;
        }

        public string GetFirstLastName()
        {
            return FirstName + " " + LastName;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            base.Configuration.ProxyCreationEnabled = false;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<UPPHercegovina.WebApplication.Models.Township> PlaceOfResidences { get; set; }

        public System.Data.Entity.DbSet<UPPHercegovina.WebApplication.Models.ProductType> ProductTypes { get; set; }

        public System.Data.Entity.DbSet<UPPHercegovina.WebApplication.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<UPPHercegovina.WebApplication.Models.Membership> Memberships { get; set; }

        public System.Data.Entity.DbSet<UPPHercegovina.WebApplication.Models.UserMembership> UserMemberships { get; set; }

        public System.Data.Entity.DbSet<UPPHercegovina.WebApplication.Models.Post> Posts { get; set; }

        public System.Data.Entity.DbSet<UPPHercegovina.WebApplication.Models.PostCategory> PostCategories { get; set; }

    }
}