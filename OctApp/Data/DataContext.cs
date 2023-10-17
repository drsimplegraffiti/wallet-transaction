using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OctApp.Models;

namespace OctApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.Property(e => e.Balance)
                    .HasColumnType("decimal(18, 2)"); // Specify the precision and scale as needed
            });

            // modelBuilder.Entity<Wallet>()
            //     .HasOne(w => w.User)
            //     .WithMany(u => u.Wallets)
            //     .HasForeignKey(w => w.UserId)
            //     .OnDelete(DeleteBehavior.Cascade);

            
        }
    }
}
