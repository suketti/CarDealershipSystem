using DealershipSystem;

public class AdminUserUpdateDTO
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string NameKanji { get; set; }
    public string PhoneNumber { get; set; }
    public string PreferredLanguage { get; set; }
    public string Role { get; set; }
    public LocationDto? Location { get; set; }
}