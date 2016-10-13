using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using UPPHercegovina.WebApplication.Abstractions;
using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Repositories
{
    public class MarkRepository : IMarkRepository, IDisposable
    {
        ApplicationDbContext context;

        public MarkRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Mark GetMarkById(int id)
        {
            return context.Marks.Find(id);
        }

        public IEnumerable<Mark> GetMarks()
        {
            return context.Marks.ToList();
        }

        public void InsertMark(Mark mark)
        {
            context.Marks.Add(mark);
        }

        public void UpdateMark(Mark mark)
        {
            context.Entry(mark).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}