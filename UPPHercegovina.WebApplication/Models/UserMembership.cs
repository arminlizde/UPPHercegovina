using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace UPPHercegovina.WebApplication.Models
{
    [DataContractAttribute(Name = "UserMembership")]
    public class UserMembership
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Članska")]
        [DataMember(Name = "MembershipId")]
        public int MembershipId { get; set; }

        [Display(Name = "Korisnik")]
        [DataMember(Name = "UserId")]
        public string UserId { get; set; }

        [Required]
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
        public virtual ApplicationUser User { get; set; }

        [DataMember(Name = "Membership")]
        public virtual Membership Membership { get; set; }

        [Display(Name = "Preostalo dana")]
        public int DaysLeft
        {
            get
            {
                var days = (int)(DateOfPayment.AddYears(1) - DateTime.Now).TotalDays;
                return days > 0 ? days : 0;
            }
        }

        [Display(Name = "Datum isteka")]
        public DateTime ExparationDate { get { return DateOfPayment.AddYears(1); } }

    }
}