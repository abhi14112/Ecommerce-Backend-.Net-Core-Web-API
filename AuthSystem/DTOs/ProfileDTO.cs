using AuthSystem.Models;

namespace AuthSystem.DTOs
{
    public class ProfileDTO
    {
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public string? gender { get; set; }
    }
}
