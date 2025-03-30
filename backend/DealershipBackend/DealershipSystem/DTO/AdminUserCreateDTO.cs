namespace DealershipSystem.DTO
{
    public class AdminUserCreateDTO
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Name { get; set; } // Added Name

        public string NameKanji { get; set; } // Added NameKanji

        public string PhoneNumber { get; set; } // Added PhoneNumber

        public string PreferredLanguage { get; set; } // Added PreferredLanguage

        public string Role { get; set; }

        public LocationDto? Location { get; set; } // LocationDto for the location
    }
}