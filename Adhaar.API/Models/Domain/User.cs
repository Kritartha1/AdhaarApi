using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adhaar.API.Models.Domain
{
    public class User:IdentityUser
    {

        public Guid? ImageId { get; set; }
        public ImageAd? Image { get; set; }

        


    }
}
