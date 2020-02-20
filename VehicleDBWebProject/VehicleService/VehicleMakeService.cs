using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleService
{
    public class VehicleMakeService : IVehicleMakeService
    {
        private readonly VehicleDBContext _VehicleDB;
        public VehicleMakeService(VehicleDBContext VehicleDB)
        {
            _VehicleDB = VehicleDB;
        }

        public async Task<VehicleMake> AddVehicleMakerAsync(VehicleMake VehicleMake)
        {
            using (var db = _VehicleDB)
            {
                db.VehicleMakes.Add(VehicleMake);
                await db.SaveChangesAsync();
                return VehicleMake;
            }

        }

        public IQueryable<VehicleMake> GetVehicleMakes()
        {
            return _VehicleDB.VehicleMakes.AsNoTracking();
        }

        public async Task<VehicleMake> DeleteVehicleMakerAsync(int Id)
        {
            using (var db = _VehicleDB)
            {
                VehicleMake make = db.VehicleMakes.Find(Id);
                db.VehicleMakes.Remove(make);
                await db.SaveChangesAsync();
                return make;
            }
        }

        public async Task<VehicleMake> GetOneVehicleMakerAsync(int Id)
        {
            return await _VehicleDB.VehicleMakes.FindAsync(Id);
        }

        public async Task<VehicleMake> UpdateAsync(VehicleMake MakeChanged)
        {
            using (var db = _VehicleDB)
            {
                var make = db.VehicleMakes.Attach(MakeChanged);
                make.State = EntityState.Modified;
                await db.SaveChangesAsync();
                return MakeChanged;
            }
        }

        public IQueryable<VehicleMake> VehicleMakeFindByName(string SearchName)
        {
            return _VehicleDB.VehicleMakes.Where(find => find.Name.Contains(SearchName));
        }
    }
}
