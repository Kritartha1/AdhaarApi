using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Adhaar.API.Models.DTO
{
    public class AddImageRequestDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public int Age { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public string Locality { get; set; }

        public string District { get; set; }

        public string State { get; set; }


        public string UID { get; set; }

        public IFormFile File { get; set; }
    }
}
