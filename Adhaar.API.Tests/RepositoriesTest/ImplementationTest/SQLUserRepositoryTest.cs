using Adhaar.API.Data;
using Adhaar.API.Models.Domain;
using Adhaar.API.Repositories.Implementaion;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adhaar.API.Tests.RepositoriesTest.ImplementationTest
{
    [TestFixture]
    public class SQLUserRepositoryTests
    {
        private AdhaarApiDbContext dbContext;
        private SQLUserRepository userRepository;

        [SetUp]
        public void Setup()
        {
            // You can use an in-memory database for testing
            var options = new DbContextOptionsBuilder<AdhaarApiDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            dbContext = new AdhaarApiDbContext(options);
            userRepository = new SQLUserRepository(dbContext);
        }

        [Test]
        public async Task CreateAsync_ValidUser_ReturnsUser()
        {
            // Arrange
            var user = new User { Id = "userId", UserName = "testUser", Email = "test@example.com" };

            // Act
            var result = await userRepository.CreateAsync(user);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(user.UserName, result.UserName);
        }

        [Test]
        public async Task DeleteAsync_ExistingUser_ReturnsUser()
        {
            // Arrange
            var user = new User { Id = "user", UserName = "testUser", Email = "test@example.com" };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await userRepository.DeleteAsync("user");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(user.UserName, result.UserName);
        }

        [Test]
        public async Task DeleteAsync_NonExistingUser_ReturnsNull()
        {
            // Act
            var result = await userRepository.DeleteAsync("nonExistingUserId");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllAsync_ReturnsListOfUsers()
        {
            // Arrange
            await dbContext.Users.AddAsync(new User { Id = "user1", UserName = "user1", Email = "user1@example.com" });
            await dbContext.Users.AddAsync(new User { Id = "user2", UserName = "user2", Email = "user2@example.com" });
            await dbContext.SaveChangesAsync();

            // Act
            var result = await userRepository.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
        }

        [Test]
        public async Task GetByIdAsync_ExistingUser_ReturnsUser()
        {
            // Arrange
            var user = new User { Id = "userI", UserName = "testUser", Email = "test@example.com" };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await userRepository.GetByIdAsync("userI");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(user.UserName, result.UserName);
        }

        [Test]
        public async Task GetByIdAsync_NonExistingUser_ReturnsNull()
        {
            // Act
            var result = await userRepository.GetByIdAsync("nonExistingUserId");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task UpdateAsync_ExistingUser_ReturnsUpdatedUser()
        {
            // Arrange
            var user = new User { Id = "use", UserName = "testUser", Email = "test@example.com" };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var updatedUser = new User { UserName = "updatedUser", Email = "updated@example.com" };

            // Act
            var result = await userRepository.UpdateAsync("use", updatedUser);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updatedUser.UserName, result.UserName);
            Assert.AreEqual(updatedUser.Email, result.Email);
        }

        [Test]
        public async Task UpdateAsync_NonExistingUser_ReturnsNull()
        {
            // Arrange
            var user = new User { Id = "userId", UserName = "testUser", Email = "test@example.com" };

            // Act
            var result = await userRepository.UpdateAsync("nonExistingUserId", user);

            // Assert
            Assert.IsNull(result);
        }

        
    }

}
