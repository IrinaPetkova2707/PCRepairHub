using BusinessLayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<RepairTicket> RepairTickets { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Preventing cascade delete from Category to RepairTicket
            builder.Entity<RepairTicket>()
                .HasOne(rt => rt.Category)
                .WithMany(c => c.RepairTickets)
                .HasForeignKey(rt => rt.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            //Preventing cascade delete from AppUser to RepairTicket
            builder.Entity<RepairTicket>()
                .HasOne(rt => rt.Client)
                .WithMany(u => u.RepairTickets)
                .HasForeignKey(rt => rt.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            //Seeding initial data for Categories
            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Hardware Diagnostics", Description = "Full system check and fault identification." },
                new Category { Id = 2, Name = "Display & Screen Replacement", Description = "Laptop screen, hinge, and display cable repairs." },
                new Category { Id = 3, Name = "Cleaning & Thermal Paste", Description = "Dust removal and thermal compound replacement." },
                new Category { Id = 4, Name = "OS & Software Setup", Description = "Operating system installation, drivers, and virus removal." }
            );
        }
    }
}