using System.ComponentModel.DataAnnotations.Schema;

namespace Adhaar.API.Models.Domain
{
    public class Image
    {
      

        [NotMapped]

        public IFormFile File { get; set; }

        public string FirstName { get; set; }

        public string FileName { get; set; }
        public string FileExtension { get; set; }

        public string FileSize { get; set; }

        //public string FileExtension { get; set; }
    }
}
