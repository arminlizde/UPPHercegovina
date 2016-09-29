using System.Collections.Generic;
using System.Linq;

using UPPHercegovina.WebApplication.Abstractions;
using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Helpers
{
    public class UserMembershipIndexSorter : Sorter
    {
        private string _sortOrder;

        public UserMembershipIndexSorter(string sortOrder)
        {
            _sortOrder = sortOrder;
        }

        public override List<UserMembershipIndexModel> Sort(List<UserMembershipIndexModel> list)
        {
            switch (_sortOrder)
            {
                case "user_desc":
                    return list.OrderByDescending(m => m.UserFirstLastName)
                         .ThenBy(m => m.Status).ThenBy(m => m.Approved)
                         .ThenBy(m => m.MembershipName).ThenBy(m => m.Date).ToList();
                case "user":
                    return list.OrderBy(m => m.UserFirstLastName)
                         .ThenBy(m => m.Status).ThenBy(m => m.Approved)
                         .ThenBy(m => m.MembershipName).ThenBy(m => m.Date).ToList();
                case "status_desc":
                    return list.OrderByDescending(m => m.Status)
                        .ThenBy(m => m.Approved).ThenBy(m => m.MembershipName)
                        .ThenBy(m => m.Date).ThenBy(m => m.UserFirstLastName).ToList();
                case "status":
                    return list.OrderBy(m => m.Status)
                        .ThenBy(m => m.Approved).ThenBy(m => m.MembershipName)
                        .ThenBy(m => m.Date).ThenBy(m => m.UserFirstLastName).ToList();
                case "membership_desc":
                    return list.OrderByDescending(m => m.MembershipName)
                        .ThenBy(m => m.Status).ThenBy(m => m.Approved)
                        .ThenBy(m => m.Date).ThenBy(m => m.UserFirstLastName).ToList();
                case "membership":
                    return list.OrderBy(m => m.MembershipName)
                        .ThenBy(m => m.Status).ThenBy(m => m.Approved)
                        .ThenBy(m => m.Date).ThenBy(m => m.UserFirstLastName).ToList();
                case "date_desc":
                    return list.OrderByDescending(m => m.Date)
                        .ThenBy(m => m.Status).ThenBy(m => m.Approved)
                        .ThenBy(m => m.MembershipName).ThenBy(m => m.UserFirstLastName).ToList();
                case "date":
                    return list.OrderBy(m => m.Date)
                         .ThenBy(m => m.Status).ThenBy(m => m.Approved)
                         .ThenBy(m => m.MembershipName).ThenBy(m => m.UserFirstLastName).ToList();
                case "approved_desc":
                    return list.OrderByDescending(m => m.Approved)
                        .ThenBy(m => m.Status).ThenBy(m => m.Date)
                        .ThenBy(m => m.MembershipName).ThenBy(m => m.UserFirstLastName).ToList();
                case "approved":
                    return list.OrderBy(m => m.Approved)
                         .ThenBy(m => m.Status).ThenBy(m => m.Date)
                         .ThenBy(m => m.MembershipName).ThenBy(m => m.UserFirstLastName).ToList();
                default:
                    return list.OrderBy(m => m.Status)
                        .ThenBy(m => m.Approved).ThenBy(m => m.MembershipName)
                        .ThenBy(m => m.Date).ThenBy(m => m.UserFirstLastName).ToList();
                   
            }
        }


    }
}