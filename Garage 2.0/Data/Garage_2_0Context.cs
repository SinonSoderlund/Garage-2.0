using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Garage_2._0.Models.Entities;

namespace Garage_2._0.Data
{
    public class Garage_2_0Context : DbContext
    {
        public Garage_2_0Context(DbContextOptions<Garage_2_0Context> options)
            : base(options)
        {
        }

        public DbSet<Garage_2._0.Models.Entities.Vehicle> Vehicle { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vehicle>().HasData(
             new Vehicle
             {
                 Id = 1,
                 Wheels = 4,
                 ArriveTime = DateTime.Parse("2024-10-30"),
                 Color = "Red",
                 RegNr = "abc123",
                 Model = "F40",
                 Brand = "Ferrari",
                 VehicleType = VehicleType.Car
             },
             new Vehicle
             {
                 Id = 2,
                 Wheels = 4,
                 ArriveTime = DateTime.Parse("2024-10-30"),
                 Color = "Yellow",
                 RegNr = "def456",
                 Model = "Supra",
                 Brand = "Toyota",
                 VehicleType = VehicleType.Car
             });
        }
    }
}
