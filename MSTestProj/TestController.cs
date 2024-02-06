using Adhaar.API.Controllers;
using Adhaar.API.Data;
using Adhaar.API.Models.Domain;
using Adhaar.API.Models.DTO;
using Adhaar.API.Repositories.Implementaion;
using Adhaar.API.Repositories.Interface;
using AutoMapper;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTestProj
{
    [TestClass]
    public class TestController
    {
        [TestMethod]
        public async Task GetById()
        {
            // Arrange

            var loggerMock = new Mock<ILogger<AuthController>>();
           var  userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
           var  tokenRepositoryMock = new Mock<ITokenRepository>();
           var  mapperMock = new Mock<IMapper>();
           var  dbContextMock = new Mock<AdhaarApiDbContext>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var fileSystemMock = new Mock<IFileSystem>();

           AuthController authController = new AuthController(loggerMock.Object, userManagerMock.Object, tokenRepositoryMock.Object, mapperMock.Object, dbContextMock.Object, userRepositoryMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext(),
                }
            };

           
            var id = "userId";
            var img = Guid.NewGuid();
            var userDomain = new User
            {
                Id = id,
                ImageId = img
            };

            userRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(userDomain);

            mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>()))
                .Returns(new UserDto { Id = id, ImageId = img, Image = null });

            // Act
            var result = await authController.GetById(id) as OkObjectResult;

            Assert.AreEqual(img,( result.Value as UserDto ).ImageId);
         


        }




    }
}
