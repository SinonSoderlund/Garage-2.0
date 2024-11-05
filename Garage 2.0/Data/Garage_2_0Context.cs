using System;using Microsoft.EntityFrameworkCore;
using Garage_2._0.Models.Entities;

namespace Garage_2._0.Data
{
    public class Garage_2_0Context : DbContext
    {
        public Garage_2_0Context(DbContextOptions<Garage_2_0Context> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicle { get; set; } = default!;
        public DbSet<Spot> Spots { get; set; } = default!;
        
        public DbSet<VehicleSpot > VehicleSpots { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vehicle>()
                .HasMany(e => e.Spots)
                .WithMany(e => e.Vehicles)
                .UsingEntity<VehicleSpot>();
            
            modelBuilder.Entity<Spot>()
                .HasMany(s => s.VehicleSpots)
                .WithOne(vs => vs.Spot)
                .HasForeignKey(vs => vs.SpotId);

            int GARAGE_SIZE = 15;
            for (int i = 1; i <= GARAGE_SIZE; i++)
            {
                modelBuilder.Entity<Spot>().HasData(
                    new Spot { Id = i }); // Initializing spots with no vehicles
            }
        }
    }
}
