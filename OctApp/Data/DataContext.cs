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

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<AppEnvironment> AppEnvironments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.Property(e => e.LiveBalance)
                    .HasColumnType("decimal(18, 2)"); // Specify the precision and scale as needed
            });

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.Property(e => e.TestBalance)
                    .HasColumnType("decimal(18, 2)"); // Specify the precision and scale as needed
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18, 2)"); // Specify the precision and scale as needed
            });


            //seed the app environment table 1 = live, 2 = test
            modelBuilder.Entity<AppEnvironment>().HasData(
                new AppEnvironment { Id = 1, Name = "Live", Value = 1 },
                new AppEnvironment { Id = 2, Name = "Test", Value = 2 }
            );

            // modelBuilder.Entity<Wallet>()
            //     .HasOne(w => w.User)
            //     .WithMany(u => u.Wallets)
            //     .HasForeignKey(w => w.UserId)
            //     .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
