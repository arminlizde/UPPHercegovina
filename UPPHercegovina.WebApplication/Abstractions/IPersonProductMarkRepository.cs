using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Abstractions
{
    public interface IPersonProductMarkRepository : IDisposable
    {
        IEnumerable<PersonProductMark> GetMarks(int PersonProductId);
        void InsertUserMark(int PersonProductId, int MarkId);
        void UpdateUserMark(int PersonProductId, int MarkId);
        decimal GetAverageMarkForUser(string UserId);
        decimal GetAverageMarkForProduct(int PersonProductId);

        void Save();
    }
}