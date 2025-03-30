namespace DealershipSystem.DTO;

public class DealerUserDTO : UserDTO
{
    public LocationDto Location { get; set; } // Single location for Dealer
}