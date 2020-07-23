using Domain.Base;
using Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Base
{
    public class DbContextBase : DbContext, IDbContext
    {
        public DbContextBase(DbContextOptions options) : base(options)
        {
        }

        public DbContextBase()
        {
        }

        public Action<string> Log { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }




        public void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.
                Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                long unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
                if (entity.State == EntityState.Modified)
                {
                    ((BaseEntity)entity.Entity).UpdatedAt = unixTimestamp;
                }
                else
                {
                    ((BaseEntity)entity.Entity).CreatedAt = unixTimestamp;
                    ((BaseEntity)entity.Entity).UpdatedAt = unixTimestamp;

                }
            }
        }



        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        



    }
}
