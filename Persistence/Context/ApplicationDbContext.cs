using Domain.Entities;
using Domain.Entities.CustomAttributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public DbSet<Item> Item { get; set; }
        public DbSet<User> User { get; set; }

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
                        modelBuilder.Entity(entityType.ClrType)
                            .ToView(entityType.ClrType.Name)
                            .HasNoKey();
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
