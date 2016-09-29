using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Helpers
{
    public class ApplicationUserHelper
    {
        //ako je samo koristim u jendoj klasi onda je treba prebacit u tu klasu... 
        internal static List<UserMembershipViewModel> GetUserSelectList()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var userlist = context.Users.OrderBy(u => u.FirstName).ThenBy(u => u.LastName).ToList();
                var userselectlist = new List<UserMembershipViewModel>();

                userlist.ForEach(user =>
                {
                    var usermembership = new UserMembershipViewModel()
                    {
                        UserId = user.Id,
                        Name = user.GetDisplayName()
                    };
                    userselectlist.Add(usermembership);
                });

                return userselectlist;
            }
        }


        //prebaci u maper
        internal static UserEditViewModel MapToEditViewModel(ApplicationUser user)
        {
            return new UserEditViewModel()
            {
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                TownshipId = user.TownshipId,
                IdentificationNumber = user.IdentificationNumber,
                Address = user.Address,
                Email = user.Email,
                PictureUrl = user.PictureUrl,
                EmailConfirmed = user.EmailConfirmed
            };
        }
    }
}