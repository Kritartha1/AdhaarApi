using System.ComponentModel.DataAnnotations;

namespace Adhaar.API.Models.DTO
{
    public class VerifyUserDto
    {


        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AdhaarNumber { get; set; }
        public int? Age { get; set; }
        public string? City { get; set; }
        public string? PinCode { get; set; }
        public string? District { get; set; }
        public string? State { get; set; }

        public IFormFile Image { get; set; }

       
    }
}
