using System.Collections.Generic;
using System.Linq;

using UPPHercegovina.WebApplication.Abstractions;
using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Helpers
{
    public class MembershipSorter : Sorter
    {
        private string _sortOrder;

        public MembershipSorter(string sortOrder)
        {
            _sortOrder = sortOrder;
        }

        public override List<Membership> Sort(List<Membership> list)
        {
            switch (_sortOrder)
            {
                case "name_desc":
                    return list.OrderByDescending(m => m.Name).ThenBy(m => m.Status).ThenBy(m => m.CreationDate).ToList();
                case "Date":
                    return list.OrderBy(m => m.CreationDate).ThenBy(m => m.Status).ThenBy(m => m.Name).ToList();
                case "date_desc":
                    return list.OrderByDescending(m => m.CreationDate).ThenBy(m => m.Status).ThenBy(m => m.Name).ToList();
                case "Status":
                    return list.OrderBy(m => m.Status).ThenBy(m => m.CreationDate).ThenBy(m => m.Name).ToList();
                case "status_desc":
                    return list.OrderByDescending(m => m.Status).ThenBy(m => m.CreationDate).ThenBy(m => m.Name).ToList();
                default:
                    return list.OrderBy(m => m.Status).ThenBy(m => m.CreationDate).ThenBy(m => m.Name).ToList();
            }
        }

    }
}