using DealershipSystem.DTO;

namespace DealershipSystem.Interfaces;

public interface IReservationService
{
    Task<IEnumerable<ReservationDTO>> GetAllAsync();
    Task<ReservationDTO> GetByIdAsync(int id);
    Task<ReservationDTO> CreateAsync(CreateReservationDTO dto);
    Task<bool> DeleteAsync(int id);
}