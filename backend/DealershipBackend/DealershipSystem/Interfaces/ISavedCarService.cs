namespace DealershipSystem.Interfaces;

public interface ISavedCarService
{
    Task<List<int>> GetSavedCarsAsync(Guid userId);
    Task SaveCarAsync(Guid userId, int carId);
    Task RemoveSavedCarAsync(Guid userId, int carId); // New method
}
