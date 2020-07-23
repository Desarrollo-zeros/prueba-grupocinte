using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Interface
{
    public interface IDbContext : IDisposable
    {
        DbSet<T> Set<T>() where T : class;
        Action<string> Log { get; set; }
        EntityEntry Entry(object entity);
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        void SetModified(object entity);
        int SaveChanges();


    }
}
