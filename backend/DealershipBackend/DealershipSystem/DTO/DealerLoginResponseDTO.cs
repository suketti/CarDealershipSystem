namespace DealershipSystem.DTO;

public class DealerLoginResponseDTO
{
    public UserDTO User { get; set; }
    public string Role { get; set; }
    public LocationDto Location { get; set; }
}
