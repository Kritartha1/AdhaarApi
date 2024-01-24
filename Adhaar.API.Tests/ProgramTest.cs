
using Adhaar.API.Data;
using Adhaar.API.Repositories.Implementaion;
using Adhaar.API.Repositories.Interface;
using AutoMapper;
using global::Adhaar.API.Data;
using global::Adhaar.API.Mappings;
using global::Adhaar.API.Repositories.Implementaion;
using global::Adhaar.API.Repositories.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Linq;


namespace Adhaar.API.Tests
{


   
        [TestFixture]
        public class ServiceRegistrationTests
        {
            [Test]
            public void Services_AreRegisteredCorrectly()
            {
                // Arrange
                var configuration = new ConfigurationBuilder().Build();
                var services = new ServiceCollection();

                // Act
                services.AddDbContext<AdhaarApiDbContext>(options => options.UseSqlServer("Server=Nath-VM;Database=AdhaarApiDbNew;Trusted_Connection=True;TrustServerCertificate=True"));
                services.AddDbContext<AdhaarApiAuthDbContext>(options => options.UseSqlServer("Server=Nath-VM;Database=AdhaarApiDbNew;Trusted_Connection=True;TrustServerCertificate=True"));
                services.AddScoped<IUserRepository, SQLUserRepository>();
                services.AddScoped<ITokenRepository, TokenRepository>();
                services.AddScoped<IImageRepository, ImageRepository>();
                services.AddAutoMapper(typeof(AutoMapperprofiles));
                services.AddIdentityCore<IdentityUser>().AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<AdhaarApiAuthDbContext>()
                    .AddDefaultTokenProviders();
                services.Configure<IdentityOptions>(options =>
                {
                    // Configure Identity options
                });
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
                {
                    // Configure JwtBearer options
                });

                // Assert
                AssertServiceRegistered<IUserRepository>(services);
                AssertServiceRegistered<ITokenRepository>(services);
                AssertServiceRegistered<IImageRepository>(services);
            }

            private void AssertServiceRegistered<TService>(ServiceCollection services)
            {
                var serviceDescriptor = services.FirstOrDefault(descriptor =>
                    descriptor.ServiceType == typeof(TService));

                Assert.IsNotNull(serviceDescriptor, $"{typeof(TService)} not registered.");
            }
        }
    



}
