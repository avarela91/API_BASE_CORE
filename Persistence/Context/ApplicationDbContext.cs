using Domain.Entities;
using Domain.Entities.CustomAttributes;
using Microsoft.EntityFrameworkCore;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departmets { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /* modelBuilder.Entity<Item>()
             .ToTable("Item")
             .HasKey(i => i.ItemCode);

             modelBuilder.Entity<User>()
            .HasKey(i => i.Id_User);

             base.OnModelCreating(modelBuilder);*/
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var connectionNameAttribute = entityType.ClrType.GetCustomAttribute<ConnectionNameAttribute>();
                if (connectionNameAttribute != null)
                {
                    var connectionName = connectionNameAttribute.ConnectionName;
                    if (!string.IsNullOrEmpty(connectionName))
                    {
                        var primaryKeyProperty = entityType.ClrType.GetProperties()
                            .FirstOrDefault(p => p.GetCustomAttribute(typeof(KeyAttribute))!=null);

                            if (primaryKeyProperty != null)
                            {
                                modelBuilder.Entity(entityType.ClrType)
                               .ToTable(entityType.ClrType.Name)
                               .HasKey(primaryKeyProperty.Name);
                            }
                            else
                            {
                                modelBuilder.Entity(entityType.ClrType)
                               .ToTable(entityType.ClrType.Name)
                               .HasNoKey();
                            }

                        }
                    }
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // No configures ninguna conexión aquí, se hará dinámicamente
        }

    }
}
