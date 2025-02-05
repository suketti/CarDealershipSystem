using DealershipSystem.DTO;
using DealershipSystem.Models;

namespace DealershipSystem.Interfaces;

public interface IEngineSizeService
{
    Task<EngineSizeModelDTO> AddEngineSizeAsync(int modelId, int engineSize, int fuelTypeId);
    Task<List<EngineSizeModelDTO>> GetEnginesAsync();
    Task<IEnumerable<EngineSizeModelDTO>?> GetEngineByModelIdAsync(int modelId);
    Task<EngineSizeModelDTO?> UpdateEngineAsync(int engineId, int newEngineSize, int fuelTypeid);
    Task<bool> DeleteEnginesAsync(int engineId);
}
