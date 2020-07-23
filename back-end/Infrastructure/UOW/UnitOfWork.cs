using Domain.Interface;
using Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContext _context;

        public UnitOfWork(IDbContext context)
        {
            _context = context;
        }


        public int Commit()
        {
            return _context.SaveChanges();
        }

       



        public void Dispose()
        {
            Dispose(true);
        }
        private void Dispose(bool disposing)
        {
            if (disposing && _context != null)
            {
                ((DbContext)_context).Dispose();
                _context = null;
            }
        }
    }
}
