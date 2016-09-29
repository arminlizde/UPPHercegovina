using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UPPHercegovina.WebApplication.Abstractions;

using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Helpers
{
    public class SortFactory
    {
        public static Sorter Resolve(string sortOrder, Type type)
        {
            if (type == typeof(List<ProductViewModel>))
                return new ProductsSorter(sortOrder);

            if (type == typeof(List<UserMembershipIndexModel>))
                return new UserMembershipIndexSorter(sortOrder);

            if (type == typeof(List<Membership>))
                return new MembershipSorter(sortOrder);

            throw new ArgumentOutOfRangeException();
        }
    }
}