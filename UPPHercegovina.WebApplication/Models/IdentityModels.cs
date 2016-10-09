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

        public string FullName { get { return FirstName + " " + LastName; } }

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

        public DbSet<Township> PlaceOfResidences { get; set; }

        public DbSet<ProductType> ProductTypes { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Membership> Memberships { get; set; }

        public DbSet<UserMembership> UserMemberships { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<PostCategory> PostCategories { get; set; }

        public DbSet<Storing> Storings { get; set; }

        public DbSet<PersonProduct> PersonProducts { get; set; }

        public DbSet<Warehouse> Warehouses { get; set; }

        public DbSet<Warehouse1> Warehouses1 { get; set; }

        public DbSet<Field> Fields { get; set; }

        public DbSet<Quality> Qualities { get; set; }

    }
}