using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleService
{
    public interface IVehicleModelService
    {
        Task<VehicleModel> AddVehicleModelAsync(VehicleModel VehicleModel);
        Task<VehicleModel> DeleteVehicleModelAsync(int Id);
        Task<VehicleModel> GetOneVehicleModelAsync(int Id);
        IQueryable<VehicleModel> GetVehicleModels();
        IEnumerable<VehicleMake> GetMakes();
        Task<VehicleModel> UpdateAsync(VehicleModel MakeChanged);
        IQueryable<VehicleModel> VehicleModelFindByName(string SearchName);
        
    }
}