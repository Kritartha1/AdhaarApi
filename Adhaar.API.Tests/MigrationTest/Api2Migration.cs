using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using global::Adhaar.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using NUnit.Framework;
using Adhaar.API.Migrations;
using Microsoft.Extensions.DependencyInjection;


namespace Adhaar.API.Tests.MigrationTest
{



   

    
        [TestFixture]
        public class MigrationTests2
        {
            private IServiceProvider _serviceProvider;

            [SetUp]
            public void Setup()
            {
                // Set up an in-memory database
                var services = new ServiceCollection();
                services.AddDbContext<AdhaarApiDbContext>(options =>
                    options.UseInMemoryDatabase(databaseName: "TestDatabase"));

                _serviceProvider = services.BuildServiceProvider();

                using (var context = _serviceProvider.GetRequiredService<AdhaarApiDbContext>())
                {
                    context.Database.EnsureCreated();
                }
            }

            [Test]
            public void ApplyMigration_Up()
            {
                // Arrange
                var migrator = CreateMigrator();

                // Act
                migrator.Migrate();

                // Assert
                // Perform assertions based on your expected changes in the database after applying the migration
            }

            /*[Test]
            public void RevertMigration_Down()
            {
                // Arrange
                var migrator = CreateMigrator();

                // Act
                migrator.Migrate(); // Apply the migration first

                // Revert the migration
                var migration = new _2ndMigration();
                migration.Down(migrator.Provider.GetService<IMigrationsBuilder>());

                // Assert
                // Perform assertions based on your expected changes in the database after reverting the migration
            }*/

            private IMigrator CreateMigrator()
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<AdhaarApiDbContext>();
                    return context.Database.GetService<IMigrator>();
                }
            }
        }
    

}


