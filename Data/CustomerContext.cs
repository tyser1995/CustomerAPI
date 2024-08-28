using CustomerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerAPI.Data
{
    public class CustomerContext:DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .OwnsMany(c => c.ContactNumbers, cb =>
                {
                    cb.WithOwner().HasForeignKey("CustomerId");
                    cb.Property(p => p.Type).HasMaxLength(20);
                    cb.Property(p => p.Number).HasMaxLength(20);
                });

            modelBuilder.Entity<Customer>()
                .OwnsMany(c => c.Addresses, ab =>
                {
                    ab.WithOwner().HasForeignKey("CustomerId");
                    ab.Property(p => p.Barangay).HasMaxLength(50);
                    ab.Property(p => p.City).HasMaxLength(50);
                    ab.Property(p => p.Province).HasMaxLength(50);
                });
        }


    }
}
