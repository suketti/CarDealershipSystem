using DealershipSystem.DTO;

public interface IReservationService
{
    Task<IEnumerable<ReservationDTO>> GetAllAsync(string userId);
    Task<ReservationDTO?> GetByIdAsync(int id, string userId);
    Task<ReservationDTO?> CreateAsync(CreateReservationDTO dto, string userId);
    Task<ReservationDTO?> UpdateAsync(int id, UpdateReservationDTO dto, string userId);
    Task<bool> DeleteAsync(int id, string userId, bool isAdmin);
}