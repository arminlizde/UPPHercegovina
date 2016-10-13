using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Abstractions
{
    public interface IMarkRepository : IDisposable
    {
        IEnumerable<Mark> GetMarks();
        Mark GetMarkById(int id);
        void InsertMark(Mark mark);
        void UpdateMark(Mark mark);
        void Save();
    }
}