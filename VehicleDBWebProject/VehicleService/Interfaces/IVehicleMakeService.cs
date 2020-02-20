using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleService
{
    public interface IVehicleMakeService
    {
        Task<VehicleMake> AddVehicleMakerAsync(VehicleMake VehicleMake);
        Task<VehicleMake> DeleteVehicleMakerAsync(int Id);
        Task<VehicleMake> GetOneVehicleMakerAsync(int Id);
        IQueryable<VehicleMake> GetVehicleMakes();
        Task<VehicleMake> UpdateAsync(VehicleMake MakeChanged);
        IQueryable<VehicleMake> VehicleMakeFindByName(string SearchName);

    }
}