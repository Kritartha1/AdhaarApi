namespace Adhaar.API.Models.DTO
{
    public class LoginResponseDto
    {
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public string Id { get; set; }
        public string JwtToken { get; set; }
    }
}
