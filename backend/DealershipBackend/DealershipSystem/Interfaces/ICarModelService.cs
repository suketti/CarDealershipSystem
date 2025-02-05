using DealershipSystem.DTO;
using DealershipSystem.Models;

namespace DealershipSystem.Interfaces;

public interface ICarModelService
{
    Task<CarModelDTO> CreateCarModelAsync(CreateCarModelDTO carModel);
    Task<CarModelDTO?> GetCarModelAsync(int id);

    Task<List<CarModelDTO>> GetCarModelsFilteredAsync(int? makerID = null, int? startYear = null, int? endYear = null,
        int? passengerCount = null);
    Task<List<CarModelDTO>> GetAllCarModelsAsync();
    Task<CarModelDTO?> UpdateCarModelAsync(int id, UpdateCarModelDTO updatedCarModel);
    Task<bool> DeleteCarModelAsync(int id);
}