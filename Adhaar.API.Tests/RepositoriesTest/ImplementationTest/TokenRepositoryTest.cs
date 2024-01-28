using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Adhaar.API.Repositories;
using global::Adhaar.API.Repositories.Implementaion;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;

namespace Adhaar.API.Tests.RepositoriesTest.ImplementationTest
{
   
   

   
        [TestFixture]
        public class TokenRepositoryTests
        {
            private TokenRepository _tokenRepository;
            private IConfiguration _configuration;

            [SetUp]
            public void Setup()
            {
                // Mock IConfiguration or provide a configuration with necessary Jwt settings
                _configuration = new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string>
                    {
                    {"Jwt:Key",  "djnvkjnewkdsnvoeloSIJINDVNNijvknd122dkn#kn@nvkJJFNn73451"},
                    {"Jwt:Issuer", "https://localhost:7006"},
                    {"Jwt:Audience", "https://localhost:7006"}

                       
                    })
                    .Build();

                _tokenRepository = new TokenRepository(_configuration);
            }

            [Test]
            public void CreateJWTToken_ReturnsValidToken()
            {
                // Arrange
                var user = new IdentityUser { Email = "test@example.com" };
                var roles = new List<string> { "Admin", "User" };

                // Act
                var token = _tokenRepository.CreateJWTToken(user, roles);

                // Assert
                Assert.IsNotEmpty(token);
                Assert.IsTrue(IsValidJwtToken(token));
            }

            private bool IsValidJwtToken(string token)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"]
                };

                try
                {
                    tokenHandler.ValidateToken(token, validationParameters, out _);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    

}
