using System.Collections.Generic;
using System.Diagnostics;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Contexts
{
    public class CityInfoContext : DbContext
    {
        public CityInfoContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
            var canConnect = Database.CanConnect();
            Debug.WriteLine($"Can connect {canConnect}");
            // options.
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointOfInterests { get; set; }

        //Either use this method or the constructor
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.
        //     base.OnConfiguring(optionsBuilder);
        // }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>()
                .HasData(
                    new City()
                    {
                        Id = 1,
                        Name = "New York City",
                        Description = "The one with that big park.",
                    }
                );

            modelBuilder.Entity<PointOfInterest>()
                .HasData(
                    new PointOfInterest()
                    {
                        Id = 1,
                        Name = "Central Park",
                        Description = "The most visited urban park in the United States.",
                        CityId = 1
                    },
                    new PointOfInterest()
                    {
                        Id = 2,
                        Name = "Empire State Building",
                        Description = "A 102-story skyscraper located in Miwn Manhattan.",
                        CityId = 1
                    }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}