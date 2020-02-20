using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleService
{
    public class VehicleModelService : IVehicleModelService
    {
        private readonly VehicleDBContext _VehicleDB;
        public VehicleModelService(VehicleDBContext VehicleDb)
        {
            _VehicleDB = VehicleDb;
        }

        public async Task<VehicleModel> AddVehicleModelAsync(VehicleModel VehicleModel)
        {
            using (var db = _VehicleDB)
            {
                db.VehicleModels.Add(VehicleModel);
                await db.SaveChangesAsync();
                return VehicleModel;
            }

        }

        public IQueryable<VehicleModel> GetVehicleModels()
        {
            return _VehicleDB.VehicleModels.AsNoTracking();
        }

        public async Task<VehicleModel> DeleteVehicleModelAsync(int Id)
        {
            using (var db = _VehicleDB)
            {
                VehicleModel model = db.VehicleModels.Find(Id);
                db.VehicleModels.Remove(model);
                await db.SaveChangesAsync();
                return model;
            }
        }

        public async Task<VehicleModel> GetOneVehicleModelAsync(int Id)
        {
            return await _VehicleDB.VehicleModels.FindAsync(Id);
        }

        public async Task<VehicleModel> UpdateAsync(VehicleModel MakeChanged)
        {
            using (var db = _VehicleDB)
            {
                var model = db.VehicleModels.Attach(MakeChanged);
                model.State = EntityState.Modified;
                await db.SaveChangesAsync();
                return MakeChanged;
            }
        }

        public IQueryable<VehicleModel> VehicleModelFindByName(string SearchName)
        {
            return _VehicleDB.VehicleModels.Where(find => find.Name.Contains(SearchName));
        }

        public IEnumerable<VehicleMake> GetMakes()
        {
            return _VehicleDB.VehicleMakes.ToList();
        }
    }
}
