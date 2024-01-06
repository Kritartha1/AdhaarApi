using Adhaar.API.Models.Domain;

namespace Adhaar.API.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();

        // Task<User?> GetByIdAsync(Guid id);

        Task<User?> GetByIdAsync(string id);

        Task<User> CreateAsync(User user);

        /*Task<User?> UpdateAsync(Guid id, User user);

        Task<User?> DeleteAsync(Guid id);*/

        Task<User?> UpdateAsync(string id, User user);

        Task<User?> DeleteAsync(string id);
    }
}
