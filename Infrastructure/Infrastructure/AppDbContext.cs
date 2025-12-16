using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Application;

namespace Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {        
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Domain.Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.User)
                .WithMany(u => u.Loans)
                .HasForeignKey(l => l.UserId);
        }
    }
}
