using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Garage_2._0.Models.Entities;
using Garage_2._0.Models.ViewModels;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Garage_2._0.Data
{
    public class Garage_2_0Context : IdentityDbContext<User, IdentityRole, string>
    {


        public DbSet<Vehicle> Vehicle { get; set; } = default!;
        public DbSet<Spot> Spots { get; set; } = default!;

        public DbSet<SpotAllocation> SpotAllocations { get; set; } = default!;  
        public DbSet<VehicleType> VehicleTypes { get; set; } = default!;

        public DbSet<FeedbackMessage> FeedbackMessage { get; set; } = default!;

        public DbSet<User> ProgramUser { get; set; } = default!;

        public Garage_2_0Context(DbContextOptions<Garage_2_0Context> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            modelBuilder.Entity<User>()
    .HasMany(u => u.Vehicles)
    .WithOne(v => v.User)
    .HasForeignKey(v => v.UserId)
    .OnDelete(DeleteBehavior.Cascade);


            int GARAGE_SIZE = 8;
            for (int i = 1; i <= GARAGE_SIZE; i++)
            {
                modelBuilder.Entity<Spot>().HasData(
                    new Spot { Id = i }); // Initializing spots with no vehicles
            }
            modelBuilder.Entity<FeedbackMessage>().HasKey(s => s.Id);
        }
    }
}
