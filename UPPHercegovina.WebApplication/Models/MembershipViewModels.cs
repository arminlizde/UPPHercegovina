using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using UPPHercegovina.WebApplication.Abstractions;
using UPPHercegovina.WebApplication.Helpers;

namespace UPPHercegovina.WebApplication.Models
{
    [DataContractAttribute(Name = "UserMembership")]
    public class UserMembershipViewModel
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Id korisnika")]
        [DataMember(Name = "UserId")]
        public string UserId { get; set; }

        [Display(Name = "Id članske")]
        [DataMember(Name = "MembershipId")]
        public int MembershipId { get; set; }

        [Display(Name = "Korisnik")]
        public string Name { get; set; }

        [Display(Name = "Datum uplate")]
        [DataMember(Name = "DateOfPayment")]
        public DateTime DateOfPayment { get; set; }

        [Display(Name = "Slika")]
        [DataMember(Name = "PictureUrl")]
        public string PictureUrl { get; set; }

        [Display(Name = "Potvrđeno")]
        [DataMember(Name = "Approved")]
        public bool Approved { get; set; }

        [Display(Name = "Status")]
        [DataMember(Name = "Status")]
        public bool Status { get; set; }

        [DataMember(Name = "User")]
        public ApplicationUser User { get; set; }
    }

    [DataContractAttribute(Name = "UserMembership")]
    public class UserMembershipIndexModel
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Datum")]
        [DataMember(Name = "DateOfPayment")]
        public DateTime Date { get; set; }

        [Display(Name = "Potvrđeno")]
        [DataMember(Name = "Approved")]
        public bool Approved { get; set; }

        [Display(Name = "Status")]
        [DataMember(Name = "Status")]
        public bool Status { get; set; }

        [Display(Name = "Broj članske")]
        public int MembershipId { get { return Membership.Id; } }

        [Display(Name = "Članska")]
        public string MembershipName { get { return Membership.Name; } }

        [Display(Name = "Korisnik")]
        public string UserFirstLastName { get { return User.GetFirstLastName(); } }

        [DataMember(Name = "User")]
        public ApplicationUser User { get; set; }

        [DataMember(Name = "Membership")]
        public Membership Membership { get; set; }
    }

    [DataContractAttribute(Name = "UserMembership")]
    public class UserMembershipEditViewModel
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Datum uplate")]
        [DataMember(Name = "DateOfPayment")]
        public DateTime DateOfPayment { get; set; }

        [Display(Name = "Slika")]
        [DataMember(Name = "PictureUrl")]
        public string PictureUrl { get; set; }

        [Display(Name = "Potvrđeno")]
        [DataMember(Name = "Approved")]
        public bool Approved { get; set; }

        [Display(Name = "Status")]
        [DataMember(Name = "Status")]
        public bool Status { get; set; }

        [Display(Name = "Korisnik")]
        public string UserFirstLastName { get { return User.GetFirstLastName(); } }

        [Display(Name = "Članska")]
        public string MembershipName { get { return Membership.Name; } }

        [DataMember(Name = "User")]
        public ApplicationUser User { get; set; }

        [DataMember(Name = "Membership")]
        public Membership Membership { get; set; }

    }

    public class UserMembershipStatsViewModel
    {
        public string UserId { get; set; }

        [Display(Name = "Ime")]
        public string Name { get; set; }

        [Display(Name = "Id Članske")]
        public int UserMembershipId { get; set; }

        [Display(Name = "Članska")]
        public string UserMembership { get; set; }

        [Display(Name = "Datum Uplate")]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "Datum isteka")]
        public DateTime ExpiringDate { get; set; }

        [Display(Name = "Preostalo")]
        public int DaysLeft { get; set; }

        public static UserMembershipStatsViewModel FillUserMemberShipStats(UserMembership userMembership)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {               
                var user = context.Users.Where(u => u.Id == userMembership.UserId).FirstOrDefault();
                var membership = context.Memberships.Find(userMembership.MembershipId);

                var model = new UserMembershipStatsViewModel
                {
                    UserId = user.Id,
                    Name = user.GetFirstLastName(),
                    PaymentDate = userMembership.DateOfPayment,
                    UserMembershipId = userMembership.Id,
                    ExpiringDate = userMembership.DateOfPayment.AddYears(1),
                    UserMembership = membership.Name,
                    DaysLeft = (int)(userMembership.DateOfPayment.AddYears(1) - DateTime.Now).TotalDays
                };

                if (model.DaysLeft < 0)
                {
                    model.DaysLeft = 0;                    
                    if(userMembership.Status)
                    {
                        var updateUserMembership = context.UserMemberships.Find(userMembership.Id);
                        context.UserMemberships.Attach(updateUserMembership);
                        updateUserMembership.Status = false;
                        context.SaveChanges();
                    }
                }
                else
                    model.DaysLeft = Convert.ToInt32(model.DaysLeft);

                return model;
            }

        }

        public static List<UserMembershipStatsViewModel> CreateStatsViewModelList(List<UserMembership> usermemberships)
        {
            var statsList = new List<UserMembershipStatsViewModel>();

            usermemberships.ForEach(item =>
            {
                var model = UserMembershipStatsViewModel.FillUserMemberShipStats(item);
                statsList.Add(model);
            });

            return statsList;
        }

    }
}