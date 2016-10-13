using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using UPPHercegovina.WebApplication.Abstractions;
using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Repositories
{
    /// <summary>
    /// Repository not tested
    /// </summary>
    public class PersonProductMarkRepository : IPersonProductMarkRepository, IDisposable
    {
        ApplicationDbContext context;

        public decimal GetAverageMarkForProduct(int PersonProductId)
        {
            var marks = GetMarks(PersonProductId);
                             
            decimal averageMark = 0;

            foreach (var mark in marks)
            {
                averageMark += mark.Mark.NumberValue;
            }

            averageMark /= marks.Count();

            return averageMark;
        }

        public decimal GetAverageMarkForUser(string UserId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PersonProductMark> GetMarks(int PersonProductId)
        {
            return context.PersonProductMarks
                .Where(ppm => ppm.PersonProductId == PersonProductId)
                .Include(ppm => ppm.PersonProduct)
                .Include(ppm => ppm.Mark).ToList();
        }

        public void InsertUserMark(int PersonProductId, int MarkId)
        {
            throw new NotImplementedException();
        }

        public void UpdateUserMark(int PersonProductId, int MarkId)
        {
            throw new NotImplementedException();
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