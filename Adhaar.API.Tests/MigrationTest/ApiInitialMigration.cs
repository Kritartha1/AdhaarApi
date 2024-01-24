using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Linq;
using Adhaar.API.Data;
using global::Adhaar.API.Data;
using global::Adhaar.API.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using FluentAssertions.Common;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Transactions;




namespace Adhaar.API.Tests.MigrationTest
{


    /*
        [TestFixture]
        public class MigrationTests
        {
            private AdhaarApiDbContext _context;

            [SetUp]
            public void Setup()
            {
                // Set up an in-memory database
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                var builder = new DbContextOptionsBuilder<AdhaarApiDbContext>()
                    .UseInMemoryDatabase(databaseName: "Server=Nath-VM;Database=AdhaarApiDbNew;Trusted_Connection=True;TrustServerCertificate=True")
                    .UseInternalServiceProvider(serviceProvider);

                _context = new AdhaarApiDbContext(builder.Options);
                _context.Database.EnsureCreated();

            }

            [Test]
            public void CanApplyMigrations()
            {
            // Arrange
        

            var migrations = _context.Database.GetPendingMigrations().ToList();

                // Act
                foreach (var migration in migrations)
                {
                    _context.Database.Migrate();
                }

                // Assert
                Assert.IsEmpty(_context.Database.GetPendingMigrations());
            }

        [Test]
        public void DatabaseSchemaMatchesExpectations()
        {
            // Arrange
            var migrationsAssembly = typeof(AdhaarApiDbContext).Assembly;

            // Act
            var imagesTableExists = migrationsAssembly.GetTypes()
                .Any(t => t.Name == "Image" && t.Namespace == "Adhaar.API.Data");

            var usersTableExists = migrationsAssembly.GetTypes()
                .Any(t => t.Name == "User" && t.Namespace == "Adhaar.API.Data");

            // Assert
            Assert.IsFalse(imagesTableExists);
            Assert.IsFalse(usersTableExists);
        }

        [TearDown]
            public void TearDown()
            {
                // Clean up resources
                _context.Dispose();
            }
        }*/

   

    
        [TestFixture]
        public class MigrationTests
        {
            private AdhaarApiDbContext _context;

            [SetUp]
            public void Setup()
            {
                // Set up an in-memory database
               /* var builder = new DbContextOptionsBuilder<AdhaarApiDbContext>()
                    .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                    .Options;*/
                 var builder = new DbContextOptionsBuilder<AdhaarApiDbContext>()
                   .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                   .Options;

            _context = new AdhaarApiDbContext(builder);
                _context.Database.EnsureCreated();
            }

            [Test]
            public void CanApplyMigrations()
            {
            // Arrange & Act
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            // Act
            var migrator = _context.Database.GetService<IMigrator>();
            migrator.Migrate();

            // Assert
            Assert.IsEmpty(_context.Database.GetPendingMigrations());
        }

            [Test]
            public void DatabaseSchemaMatchesExpectations()
            {
                // Arrange
                var model = _context.Model;

                // Act

                var imagesTableExists = model.FindEntityType(typeof(ImageAd)) != null;
                var usersTableExists = model.FindEntityType(typeof(User)) != null;

                // Assert
                Assert.IsTrue(imagesTableExists);
                Assert.IsTrue(usersTableExists);
            }

            [TearDown]
            public void TearDown()
            {
                // Clean up resources
                _context.Database.EnsureDeleted();
                _context.Dispose();
            }
        }
    


}
