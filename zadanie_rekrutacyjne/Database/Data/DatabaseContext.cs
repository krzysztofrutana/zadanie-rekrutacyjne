using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zadanie_rekrutacyjne.Database.Models;

namespace zadanie_rekrutacyjne.Database.Data
{
    public class DatabaseContext : IdentityDbContext
    {
        public DbSet<Category> Categories { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasOne(p => p.Parent).WithMany(c => c.CategoryChildren); 
        }
    }
}
