using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace Adhaar.API.Models.Domain
{
    public class User:IdentityUser
    {
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
