using System.Collections.Generic;
using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Abstractions
{
    public abstract class Sorter
    {
        public virtual List<ProductViewModel> Sort(List<ProductViewModel> list) 
        { return new List<ProductViewModel>(); }

       public virtual List<UserMembershipIndexModel> Sort(List<UserMembershipIndexModel> list)
        { return new List<UserMembershipIndexModel>(); }

       public virtual List<Membership> Sort(List<Membership> list)
       { return new List<Membership>(); }
    }
}