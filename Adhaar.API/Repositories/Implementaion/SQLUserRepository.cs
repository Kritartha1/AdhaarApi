using Adhaar.API.Data;
using Adhaar.API.Models.Domain;
using Adhaar.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Adhaar.API.Repositories.Implementaion
{
    public class SQLUserRepository:IUserRepository
    {
        private readonly AdhaarApiDbContext dbContext;

        public SQLUserRepository(AdhaarApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<User> CreateAsync(User user)
        {
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return user;
        }
        //public async Task<User?> DeleteAsync(Guid id)
        public async Task<User?> DeleteAsync(string id)
        {


            var existingUser = await dbContext.Users.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (existingUser == null) { return null; }

            dbContext.Users.Remove(existingUser);
            await dbContext.SaveChangesAsync();
            return existingUser;

        }

        public async Task<List<User>> GetAllAsync()
        {
            return await dbContext.Users.Include(a=>a.Image).ToListAsync();
        }

        //public async Task<User?> GetByIdAsync(Guid id)
        public async Task<User?> GetByIdAsync(string id)
        {
            //return await dbContext.Users.FirstOrDefaultAsync(x => x.Id.Trim().Equals(id, StringComparison.OrdinalIgnoreCase));
            return await dbContext.Users.Include(a=>a.Image).FirstOrDefaultAsync(x => (x.Id).Equals(id));

        }

        //public async Task<User?> UpdateAsync(Guid id, User user)
        public async Task<User?> UpdateAsync(string id, User user)
        {
            var existingUser = await dbContext.Users.FirstOrDefaultAsync(x => (x.Id).Equals(id));
            if (existingUser == null) { return null; }

            existingUser.Id = id;
            existingUser.UserName = user.UserName == null ? existingUser.UserName : user.UserName;
            existingUser.Email = user.Email == null ? existingUser.Email : user.Email;
            existingUser.PhoneNumber = user.PhoneNumber == null ? existingUser.PhoneNumber : user.PhoneNumber;
            existingUser.PasswordHash = user.PasswordHash == null ? existingUser.PasswordHash : user.PasswordHash;
            existingUser.ImageId=user.ImageId==null ? existingUser.ImageId : user.ImageId;
            existingUser.Image=user.Image==null ? existingUser.Image : user.Image;
           
            


            await dbContext.SaveChangesAsync();
            return existingUser;

        }
    }
}
