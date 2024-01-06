using Microsoft.AspNetCore.Identity;

namespace Adhaar.API.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
