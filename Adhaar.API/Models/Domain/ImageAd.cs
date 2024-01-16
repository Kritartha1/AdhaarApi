using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adhaar.API.Models.Domain
{
    public class ImageAd
    {
      
        public Guid Id { get; set; }    

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Address { get; set; }

        public int? Age { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string? Phone {get; set; }

        public string? Locality { get; set; }

        public string? District { get; set; }

        public string? State { get; set; }

       
        public string? UID { get; set; }

        [NotMapped]

        public IFormFile? File { get; set; }
       

        //public string FileExtension { get; set; }
    }
}
