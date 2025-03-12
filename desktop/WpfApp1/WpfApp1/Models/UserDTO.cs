using System;

namespace WpfApp1.Models
{
    public class UserDTO
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string NameKanji { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string PreferredLanguage { get; set; }
    }
}
