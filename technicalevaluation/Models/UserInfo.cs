using technicalevaluation.Enum;

namespace technicalevaluation.Models
{
    public class UserInfo
    {
        public int? Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public StatusUser Status { get; set; }
    }
}
