using Project.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Project.Service.DAL
{
    public class VehicleContext : DbContext
    {
        public VehicleContext()
            : base("VehicleContext")
        {

        }
        public DbSet<VehicleMake> VehicleMakes { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //imena tablica u jednini
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}