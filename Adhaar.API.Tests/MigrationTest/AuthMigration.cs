using Adhaar.API.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adhaar.API.Tests.MigrationTest
{
    [TestFixture]
    public class AdhaarApiAuthDbMigrationTests
    {
        private DbContextOptions<AdhaarApiAuthDbContext> _options;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<AdhaarApiAuthDbContext>()
                .UseSqlServer("Server=Nath-VM;Database=AdhaarApiDbNew;Trusted_Connection=True;TrustServerCertificate=True") 
                .Options;

            using (var context = new AdhaarApiAuthDbContext(_options))
            {
                context.Database.Migrate();
            }
        }

        [Test]
        public void CreateAspNetRoles_ShouldSucceed()
        {
            using (var context = new AdhaarApiAuthDbContext(_options))
            {
                // Act
                var roles = context.Roles.ToList();

                // Assert
                Assert.IsNotNull(roles);
                Assert.AreEqual(2, roles.Count); // Assuming you have 2 roles as per your migration
            }
        }

        [Test]
        public void CreateAspNetUsers_ShouldSucceed()
        {
            using (var context = new AdhaarApiAuthDbContext(_options))
            {
                // Act
                var users = context.Users.ToList();

                // Assert
                Assert.IsNotNull(users);
                // Add more assertions based on your user creation logic
            }
        }

        [Test]
        public void CreateAspNetRoleClaims_ShouldSucceed()
        {
            using (var context = new AdhaarApiAuthDbContext(_options))
            {
                // Act
                var roleClaims = context.RoleClaims.ToList();

                // Assert
                Assert.IsNotNull(roleClaims);
                // Add more assertions based on your role claims creation logic
            }
        }






        /* [Test]
         public void Down_ShouldDropAspNetUserClaimsTable()
         {
             // Arrange
             using (var context = new AdhaarApiAuthDbContext(_options))
             {
                 // Act
                 context.Database.EnsureCreated();

                 // Assert
                 Assert.That(() => context.UserClaims.Count(), Throws.Exception.TypeOf<Microsoft.EntityFrameworkCore.DbUpdateException>());
             }
         }*/

        // Add similar tests for dropping other tables

        [TearDown]
        public void Cleanup()
        {
            using (var context = new AdhaarApiAuthDbContext(_options))
            {
                context.Database.EnsureDeleted();
            }
        }
    }

}
