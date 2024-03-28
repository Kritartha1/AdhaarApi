using System.ComponentModel.DataAnnotations;

namespace Adhaar.API.Helper
{
    public class ResetPassword
    {
        [Required]
        public string Password { get; set; }

        [Compare("Password",ErrorMessage ="Password and confirm Password doesn't match")]
        public string ConfirmPassword { get; set; }

        public string Email { get; set; } = null;
        public string Token { get; set; } = null;

    }
}
