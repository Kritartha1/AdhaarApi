using System.ComponentModel.DataAnnotations;

namespace Adhaar.API.Models.DTO
{
    public class UserDto
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AdhaarNumber { get; set; }
        public int? Age { get; set; }
        public string? City { get; set; }
        public string? PinCode { get; set; }
        public string? District { get; set; }
        public string? State { get; set; }
    }
}
