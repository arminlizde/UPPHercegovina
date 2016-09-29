namespace UPPHercegovina.WebApplication.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using UPPHercegovina.WebApplication.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<UPPHercegovina.WebApplication.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "UPPHercegovina.WebApplication.Models.ApplicationDbContext";
        }

        protected override void Seed(UPPHercegovina.WebApplication.Models.ApplicationDbContext context)
        {

            //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            //var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            //// Create Admin Role
            //string roleName = "Nakupac";
            //IdentityResult roleResult;

            //// Check to see if Role Exists, if not create it
            //if (!RoleManager.RoleExists(roleName))
            //{
            //    roleResult = RoleManager.Create(new IdentityRole(roleName));
            //}

        }
    }
}
