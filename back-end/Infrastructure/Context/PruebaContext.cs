using Domain.Entities;
using Infrastructure.Base;
using Infrastructure.Seedings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Context
{
    public class PruebaContext : DbContextBase
    {

        public virtual DbSet<User> Users { set; get; }
        public virtual DbSet<TypeIdentity>  TypeIdentities { set; get; }

        public PruebaContext(DbContextOptions<PruebaContext> options) : base(options)
        {
            Database.EnsureCreated();

        }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity => {

                entity.ToTable("users");

                entity.HasOne(d => d.TypeIdentity)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_users_type_id_1");

            });

            modelBuilder.Entity<User>()
               .Property(e => e.Id)
               .ValueGeneratedOnAdd();

            modelBuilder.Entity<TypeIdentity>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();


            modelBuilder.Entity<TypeIdentity>().HasData(new TypeIdentity
            {
                Id = 1,
                Name = "Cédula de Ciudadanía",
                Type= "CC",
            }) ;

            modelBuilder.Entity<TypeIdentity>().HasData(new TypeIdentity
            {
                Id =2,
                Name = "Cédula de Extranjería",
                Type = "CE",
            });
            modelBuilder.Entity<TypeIdentity>().HasData(new TypeIdentity
            {
                Id = 3,
                Name = "Pasaporte",
                Type = "PA",
            });
            modelBuilder.Entity<TypeIdentity>().HasData(new TypeIdentity
            {
                Id = 4,
                Name = "Registro Civil",
                Type = "RC",
            });
            modelBuilder.Entity<TypeIdentity>().HasData(new TypeIdentity
            {
                Id = 5,
                Name = "Tarjeta de Identidad",
                Type = "TI",
            });


            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Name = "Carlos Andrés",
                Email = "carloscastilla31@gmail.com",
                LastName = "Castilla García",
                TypeId = 1,
                Password = new PasswordHasher().HashPassword("4015594wae"),
                IdentityNumber = "1063969856",

            });

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 2,
                Name = "test test",
                Email = "testest@gmail.com",
                LastName = "test test",
                TypeId = 1,
                Password = new PasswordHasher().HashPassword("4015594wae"),
                IdentityNumber = "1063969858",
            });
        }
    }
}
