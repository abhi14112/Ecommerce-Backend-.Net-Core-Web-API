using System.ComponentModel.DataAnnotations;
namespace AuthSystem.Models
{
    public class AddressModel
    {
        [Key]
        public int Id { get; set; }
        public string AddressLine { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set;} = string.Empty;
        public string PinCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}