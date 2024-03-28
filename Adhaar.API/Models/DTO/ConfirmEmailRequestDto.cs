namespace Adhaar.API.Models.DTO
{
    public class ConfirmEmailRequestDto
    {
        public string Token { get; set; }
        public string Email { get; set; }
    }
}
