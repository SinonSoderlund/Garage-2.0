﻿using System;
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
        public DbSet<Spot> Spots { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Spot>()
                
                // 1-1 relationship. 1 spot can have 1 vehicle and vice versa
                .HasOne(s => s.Vehicle) // 1 Spot can only have a single vehicle .
                .WithOne(v => v.Spot) // 1 Vehicle can only have a single spot.
                
                // telling entityframework that Prop VehicleId in Spot-class will serve as foreign key.
                // VehicleId FK will store Vehicle's PK (Vehicle.Id) when a spot is accociated with a vehicle.
                .HasForeignKey<Spot>(s => s.VehicleId) // specifies which prop that will serve as foreign key
                .OnDelete(DeleteBehavior.SetNull);

            int GARAGE_SIZE = 8;
            for (int i = 1; i <= GARAGE_SIZE; i++)
            {
                modelBuilder.Entity<Spot>().HasData(
                    new Spot { Id = i, VehicleId = null }); // Initializing spots with no vehicles
            }
            
            // modelBuilder.Entity<Vehicle>().HasData(
            //     new Vehicle
            //     {
            //         Id = 1,
            //         Wheels = 4,
            //         ArriveTime = DateTime.Parse("2024-10-30"),
            //         Color = "Red",
            //         RegNr = "abc123",
            //         Model = "F40",
            //         Brand = "Ferrari",
            //         VehicleType = VehicleType.Car
            //     },
            //     new Vehicle
            //     {
            //         Id = 2,
            //         Wheels = 4,
            //         ArriveTime = DateTime.Parse("2024-10-30"),
            //         Color = "Yellow",
            //         RegNr = "def456",
            //         Model = "Supra",
            //         Brand = "Toyota",
            //         VehicleType = VehicleType.Car
            //     });
        
        }
    }
}
