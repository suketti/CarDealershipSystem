using DealershipSystem.DTO;

namespace DealershipSystem.Interfaces;

public interface ICarMakerService
{
     Task<CarMakerDTO?> CreateNewMakerAsync(CreateCarMakerDTO createCarMakerDto);
     Task<List<CarMakerDTO>> GetMakersAsync();
     Task<CarMakerDTO?> GetMakerByIdAsync(int id);
     Task<bool> DeleteMakerByIdAsync(int id);
}