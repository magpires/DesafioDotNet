using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Context
{
    public class DesafioDotNetContext : DbContext
    {
        public DesafioDotNetContext() : base("DefaultConnection")
        {
           
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("Products")
                .HasKey(b => b.Id);

            modelBuilder.Entity<Product>().Property(b => b.Brand)
                .IsRequired()
                .HasMaxLength(256);

            modelBuilder.Entity<Product>().Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(256);

            base.OnModelCreating(modelBuilder);
        }
    }
}
