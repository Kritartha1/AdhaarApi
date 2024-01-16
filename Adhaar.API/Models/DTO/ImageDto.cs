using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Adhaar.API.Models.DTO
{
    public class ImageDto
    {
        public Guid Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Address { get; set; }

        public int? Age { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        public string? Locality { get; set; }

        public string? District { get; set; }

        public string? State { get; set; }


        public string? UID { get; set; }

        [NotMapped]

        public IFormFile? File { get; set; }

    }
}
