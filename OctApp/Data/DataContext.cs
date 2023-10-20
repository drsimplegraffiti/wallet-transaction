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

        // public DbSet<AppSetting> AppSettings { get; set; }
        public DbSet<Bank> Banks { get; set; }

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

            // seed the bank table
            modelBuilder.Entity<Bank>().HasData(
                new Bank { Id = 1, BankName = "Access Bank", BankCode = "044", BankId = "044" },
                new Bank { Id = 2, BankName = "Citibank", BankCode = "023", BankId = "023" },
                new Bank { Id = 3, BankName = "Diamond Bank", BankCode = "063", BankId = "063" },
                new Bank { Id = 4, BankName = "Dynamic Standard Bank", BankCode = "0", BankId = "0" },
                new Bank { Id = 5, BankName = "Ecobank Nigeria", BankCode = "050", BankId = "050" },
                new Bank { Id = 6, BankName = "Fidelity Bank Nigeria", BankCode = "070", BankId = "070" },
                new Bank { Id = 7, BankName = "First Bank of Nigeria", BankCode = "011", BankId = "011" },
                new Bank { Id = 8, BankName = "First City Monument Bank", BankCode = "214", BankId = "214" },
                new Bank { Id = 9, BankName = "Guaranty Trust Bank", BankCode = "058", BankId = "058" },
                new Bank { Id = 10, BankName = "Heritage Bank Plc", BankCode = "030", BankId = "030" },
                new Bank { Id = 11, BankName = "Jaiz Bank", BankCode = "301", BankId = "301" },
                new Bank { Id = 12, BankName = "Keystone Bank Limited", BankCode = "082", BankId = "082" },
                new Bank { Id = 13, BankName = "Providus Bank Plc", BankCode = "101", BankId = "101" },
                new Bank { Id = 14, BankName = "Polaris Bank", BankCode = "076", BankId = "076" },
                new Bank { Id = 15, BankName = "Stanbic IBTC Bank Nigeria Limited", BankCode = "221", BankId = "221" },
                new Bank { Id = 16, BankName = "Standard Chartered Bank", BankCode = "068", BankId = "068" },
                new Bank { Id = 17, BankName = "Sterling Bank", BankCode = "232", BankId = "232" },
                new Bank { Id = 18, BankName = "Suntrust Bank Nigeria Limited", BankCode = "100", BankId = "100" },
                new Bank { Id = 19, BankName = "Union Bank of Nigeria", BankCode = "032", BankId = "032" },
                new Bank { Id = 20, BankName = "United Bank for Africa", BankCode = "033", BankId = "033" },
                new Bank { Id = 21, BankName = "Unity Bank Plc", BankCode = "215", BankId = "215" },
                new Bank { Id = 22, BankName = "Wema Bank", BankCode = "035", BankId = "035" },
                new Bank { Id = 23, BankName = "Zenith Bank", BankCode = "057", BankId = "057" }
                
            );

            // modelBuilder.Entity<Wallet>()
            //     .HasOne(w => w.User)
            //     .WithMany(u => u.Wallets)
            //     .HasForeignKey(w => w.UserId)
            //     .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
