using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Project.Service.Models;

namespace Project.Service.DAL
{
    public class VehicleInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<VehicleContext>
    {
        protected override void Seed(VehicleContext context)
        {
            var vehicleMakes = new List<VehicleMake>
            {
                new VehicleMake{Name="Volkswagen", Abrv="VW"},
                new VehicleMake{Name="Bayerische Motoren Werke", Abrv="BMW"},
                new VehicleMake{Name="Chevrolet",Abrv="Unknown"}
            };
            vehicleMakes.ForEach(vmk => context.VehicleMakes.Add(vmk));
            context.SaveChanges();

            var vehicleModels = new List<VehicleModel>
            {
                new VehicleModel{VehicleMakeID=1,Name="Passat CC", Abrv="Unknown"},
                new VehicleModel{VehicleMakeID=1,Name="Golf V", Abrv="Unknown"},
                new VehicleModel{VehicleMakeID=2,Name="X6", Abrv="Unknown"},
                new VehicleModel{VehicleMakeID=3,Name="Camaro", Abrv="Unknown"}
            };
            vehicleModels.ForEach(vmd => context.VehicleModels.Add(vmd));
            context.SaveChanges();
        }
    }
}