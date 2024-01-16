using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adhaar.API.Models.Domain
{
    public class User:IdentityUser
    {

        public Guid? ImageId { get; set; }
        public ImageAd? Image { get; set; }

       /* public string? FirstName { get; set; }
        public string? LastName { get; set; }
       
        public int? Age { get; set; }
        public string? City { get; set; }
        public string? PinCode { get; set; }
        public string? District { get; set; }
        public string? State { get; set; }
        public string? Address { get; set; }

       *//* public string Phone { get; set; }*//*

        public string? Locality { get; set; }

        public string? UID { get; set; }

        [NotMapped]

        public IFormFile? File { get; set; }*/

    }
}
