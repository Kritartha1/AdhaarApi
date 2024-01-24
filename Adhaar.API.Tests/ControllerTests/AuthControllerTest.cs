using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Adhaar.API.Controllers;
using Adhaar.API.Models.Domain;
using Adhaar.API.Models.DTO;
using Adhaar.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System;

using global::Adhaar.API.Data;
using IronOcr;
using Microsoft.AspNetCore.Http;
using System.IO.Abstractions;



namespace Adhaar.API.Tests.ControllerTests
{
   

    
        [TestFixture]
        public class AuthControllerTests
        {
            private AuthController authController;
            private Mock<ILogger<AuthController>> loggerMock;
            private Mock<UserManager<IdentityUser>> userManagerMock;
            private Mock<ITokenRepository> tokenRepositoryMock;
            private Mock<IMapper> mapperMock;
            private Mock<AdhaarApiDbContext> dbContextMock;
            private Mock<IUserRepository> userRepositoryMock;
            private Mock<IFileSystem> fileSystemMock;

            [SetUp]
            public void Setup()
            {
                loggerMock = new Mock<ILogger<AuthController>>();
                userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
                tokenRepositoryMock = new Mock<ITokenRepository>();
                mapperMock = new Mock<IMapper>();
                dbContextMock = new Mock<AdhaarApiDbContext>();
                userRepositoryMock = new Mock<IUserRepository>();
                fileSystemMock = new Mock<IFileSystem>();

                authController = new AuthController(loggerMock.Object, userManagerMock.Object, tokenRepositoryMock.Object, mapperMock.Object, null, userRepositoryMock.Object)
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext(),
                    }
                };
            }

            [Test]
            public async Task Register_ValidRequest_ReturnsCreatedAtAction()
            {
                // Arrange
                var registerRequestDto = new RegisterUserRequestDto
                {
                    Username = "testuser",
                    Password = "testpassword",
                    Roles = new string[] { "User" },
                };

                userManagerMock.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                    .ReturnsAsync(IdentityResult.Success);

                userManagerMock.Setup(um => um.AddToRolesAsync(It.IsAny<IdentityUser>(), It.IsAny<IEnumerable<string>>()))
                    .ReturnsAsync(IdentityResult.Success);

                userRepositoryMock.Setup(ur => ur.CreateAsync(It.IsAny<User>()))
                    .ReturnsAsync(new User());

                mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>()))
                    .Returns(new UserDto());

                // Act
                var result = await authController.Register(registerRequestDto) as CreatedAtActionResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("GetById", result.ActionName);
                Assert.IsNull(result.RouteValues["id"]);
                Assert.IsNotNull(result.Value as UserDto);
            }

            [Test]
            public async Task Register_InvalidRequest_ReturnsBadRequest()
            {
                // Arrange
                var registerRequestDto = new RegisterUserRequestDto
                {
                    Username = "testuser",
                    Password = "testpassword",
                    Roles = new string[] { "User" },
                };

                userManagerMock.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                    .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error" }));

                // Act
                var result = await authController.Register(registerRequestDto) as BadRequestObjectResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("Oops! something went wrong!", result.Value);
            }

            [Test]
            public async Task Login_ValidRequest_ReturnsOk()
            {
                // Arrange
                var loginRequestDto = new LoginRequestDto
                {
                    Username = "testuser",
                    Password = "testpassword",
                };

                var user = new IdentityUser
                {
                    Id = "userId",
                };

                userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
                    .ReturnsAsync(user);

                userManagerMock.Setup(um => um.CheckPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                    .ReturnsAsync(true);

                userManagerMock.Setup(um => um.GetRolesAsync(It.IsAny<IdentityUser>()))
                    .ReturnsAsync(new List<string> { "User" });

                tokenRepositoryMock.Setup(tr => tr.CreateJWTToken(It.IsAny<IdentityUser>(), It.IsAny<List<string>>()))
                    .Returns("jwtToken");

                // Act
                var result = await authController.Login(loginRequestDto) as OkObjectResult;

            // Assert


            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            var responseDto = result.Value as LoginResponseDto;
            Assert.IsNotNull(responseDto);
            Assert.AreEqual("testuser", responseDto.Email);
            Assert.AreEqual("userId", responseDto.Id);
            Assert.AreEqual("jwtToken", responseDto.JwtToken);
            Assert.IsNotNull(responseDto.Roles);
            CollectionAssert.Contains(responseDto.Roles, "User");
            /* Assert.IsNotNull(result);
             Assert.AreEqual("testuser", result.Value.GetType().GetProperty("Email").GetValue(result.Value));
             Assert.IsNotNull(result.Value.GetType().GetProperty("Roles").GetValue(result.Value) as List<string>);
             Assert.AreEqual("jwtToken", result.Value.GetType().GetProperty("JwtToken").GetValue(result.Value));
             Assert.AreEqual("userId", result.Value.GetType().GetProperty("Id").GetValue(result.Value));*/
        }

            [Test]
            public async Task Login_InvalidRequest_ReturnsBadRequest()
            {
                // Arrange
                var loginRequestDto = new LoginRequestDto
                {
                    Username = "testuser",
                    Password = "testpassword",
                };

                userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
                    .ReturnsAsync((IdentityUser)null);

                // Act
                var result = await authController.Login(loginRequestDto) as BadRequestObjectResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("wrong username or password", result.Value);
            }



            [Test]
            public async Task DoOCR_ValidRequest_ReturnsOk()
            {
                // Arrange
                var id = "userId";
                var imageAd = new ImageAd
                {
                    File = new FormFile(Stream.Null, 0, 0, "testImage", "test.jpg"),
                };



            

            /*var ocrResult = new OcrResult
                {
                    Text = "Sample OCR text",
                };*/


                var ironTesseractMock = new Mock<IronTesseract>();
            /*var ocr = new IronTesseract();*/
            using var input = new OcrInput();
            input.AddImage("HAHA.png");

            OcrResult ocrResult = ironTesseractMock.Object.Read(input);
            ironTesseractMock.Setup(ocr => ocr.Read(It.IsAny<OcrInput>())).Returns(ocrResult);

                authController.SetFileSystem(fileSystemMock.Object);

                // Act
                var result = await authController.DoOCR(id, imageAd) as OkObjectResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("Sample OCR text", result.Value);
            }

            [Test]
            public async Task DoOCR_InvalidFile_ReturnsBadRequest()
            {
                // Arrange
                var id = "userId";
                var imageAd = new ImageAd
                {
                    File = null,
                };

                // Act
                var result = await authController.DoOCR(id, imageAd) as BadRequestObjectResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("No file uploaded.", result.Value);
            }

            [Test]
            public async Task DoOCR_UnsupportedFileFormat_ReturnsBadRequest()
            {
                // Arrange
                var id = "userId";
                var imageAd = new ImageAd
                {
                    File = new FormFile(Stream.Null, 0, 0, "testImage", "test.txt"),
                };

                // Act
                var result = await authController.DoOCR(id, imageAd) as BadRequestObjectResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("No file uploaded.", result.Value);
            }

            [Test]
            public async Task GetById_ExistingUser_ReturnsOk()
            {
                // Arrange
                var id = "userId";
                var userDomain = new User
                {
                    Id = id,
                    ImageId=Guid.NewGuid()
                };

                userRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<string>()))
                    .ReturnsAsync(userDomain);

                mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>()))
                    .Returns(new UserDto { Id = id,ImageId=Guid.NewGuid(),Image=null });

                // Act
                var result = await authController.GetById(id) as OkObjectResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Value as UserDto);
                Assert.AreEqual(id, result.Value.GetType().GetProperty("Id").GetValue(result.Value));
            }

            [Test]
            public async Task GetById_NonExistingUser_ReturnsNotFound()
            {
                // Arrange
                var id = "userId";

                userRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<string>()))
                    .ReturnsAsync((User)null);

                // Act
                var result = await authController.GetById(id) as NotFoundResult;

                // Assert
                Assert.IsNotNull(result);
            }
        }
    




}


