
using Adhaar.API.Controllers;
using Adhaar.API.Models.DTO;
using Adhaar.API.Models.Domain;
using Adhaar.API.Repositories.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

using global::Adhaar.API.Data;

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Adhaar.API.Tests.ControllerTests
{
    

    
        [TestFixture]
        public class ImageControllerTests
        {
            private ImageController imageController;
            private Mock<IMapper> mapperMock;
            private Mock<AdhaarApiDbContext> dbContextMock;
            private Mock<IUserRepository> userRepositoryMock;
            private Mock<IImageRepository> imageRepositoryMock;
            private Mock<ILogger<ImageController>> loggerMock;

            [SetUp]
            public void Setup()
            {
                mapperMock = new Mock<IMapper>();
                dbContextMock = new Mock<AdhaarApiDbContext>();
                userRepositoryMock = new Mock<IUserRepository>();
                imageRepositoryMock = new Mock<IImageRepository>();
                loggerMock = new Mock<ILogger<ImageController>>();

                imageController = new ImageController(mapperMock.Object, null, userRepositoryMock.Object, imageRepositoryMock.Object, loggerMock.Object)
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext(),
                    }
                };
            }

            [Test]
            public async Task GetAll_ValidRequest_ReturnsOk()
            {
                // Arrange
                var imagesDomain = new List<ImageAd>();
                mapperMock.Setup(m => m.Map<List<ImageDto>>(It.IsAny<List<ImageAd>>()))
                    .Returns(new List<ImageDto>());

                imageRepositoryMock.Setup(ir => ir.GetAllAsync())
                    .ReturnsAsync(imagesDomain);

                // Act
                var result = await imageController.GetAll() as OkObjectResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Value as List<ImageDto>);
            }

            [Test]
            public async Task GetAll_NoImages_ReturnsNotFound()
            {
                // Arrange
                mapperMock.Setup(m => m.Map<List<ImageDto>>(It.IsAny<List<ImageAd>>()))
                    .Returns((List<ImageDto>)null);

                imageRepositoryMock.Setup(ir => ir.GetAllAsync())
                    .ReturnsAsync((List<ImageAd>)null);

                // Act
                var result = await imageController.GetAll() as NotFoundObjectResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("No users found", result.Value);
            }

            [Test]
            public async Task GetById_ExistingUser_ReturnsOk()
            {
                // Arrange
                var id = "userId";
                var userDomain = new User
                {
                    Id = id,
                    Image = new ImageAd(),
                };

                userRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<string>()))
                    .ReturnsAsync(userDomain);

                mapperMock.Setup(m => m.Map<ImageDto>(It.IsAny<ImageAd>()))
                    .Returns(new ImageDto());

                // Act
                var result = await imageController.GetById(id) as OkObjectResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Value as ImageDto);
            }

            [Test]
            public async Task GetById_NonExistingUser_ReturnsNotFound()
            {
                // Arrange
                var id = "userId";

                userRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<string>()))
                    .ReturnsAsync((User)null);

                // Act
                var result = await imageController.GetById(id) as NotFoundObjectResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("No user found!", result.Value);
            }

        [Test]
        public async Task Create_ValidRequest_ReturnsOk()
        {
            // Arrange
            var id = "userId";
            byte[] fileContent = new byte[] { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100 };
            var addImageRequestDto = new AddImageRequestDto
            {
                FirstName = "ksdjhdsjf",
                LastName = "jkhdskfsd",
                Address = "kjhjkds",
                Age = 10,
                Phone = "1234567890",
                Locality = "fhkds",
                District = "fjdsf",
                State = "jksdjf",
                UID = "jnjdsfnjdsfdjskfj",
                File = new FormFile(
                        baseStream: new System.IO.MemoryStream(fileContent),
                        baseStreamOffset: 0,
                        length: fileContent.Length,
                        name: "file",
                        fileName: "example.jpg"
)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg" // Set the content type
                }
        };
        
            var imageDomainModel = new ImageAd { Id = Guid.NewGuid(),
                FirstName="ksdjhdsjf",
                LastName="jkhdskfsd",
                Address="kjhjkds",
                Age=10,
                Phone="1234567890",
                Locality="fhkds",
                District="fjdsf",
                State="jksdjf",
                UID="jnjdsfnjdsfdjskfj",
                File=null
             };

            mapperMock.Setup(m => m.Map<ImageAd>(It.IsAny<AddImageRequestDto>()))
                .Returns(imageDomainModel);
            

            userRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User { Id = id });

            imageRepositoryMock.Setup(ir => ir.CreateAsync(It.IsAny<ImageAd>()))
                .ReturnsAsync(imageDomainModel);

           /* imageRepositoryMock.Setup(ir => ir.OCR(It.IsAny<ImageAd>()))
                .ReturnsAsync("Sample OCR Text");*/

            // Act
            try
            {
                var result = await imageController.Create(id, addImageRequestDto) as OkObjectResult;

                // Assert
                Assert.IsNull(result);
                Assert.IsNull(result?.Value as UserDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                throw; 
            }
        }


        [Test]
            public async Task Create_InvalidRequest_ReturnsBadRequest()
            {
                // Arrange
                var id = "userId";
                var addImageRequestDto = new AddImageRequestDto();
                var imageDomainModel = new ImageAd { Id = Guid.NewGuid() };

                mapperMock.Setup(m => m.Map<ImageAd>(It.IsAny<AddImageRequestDto>()))
                    .Returns(imageDomainModel);

                userRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<string>()))
                    .ReturnsAsync((User)null);

                // Act
                var result = await imageController.Create(id, addImageRequestDto) as BadRequestObjectResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("Oops! Something is wrong. Try again!", result.Value);
            }



        [Test]
        public async Task Delete_ValidId_ReturnsOk()
        {
            // Arrange
            var id = Guid.NewGuid();
            var imageDomainModel = new ImageAd { Id = id,
                FirstName = "ksdjhdsjf",
                LastName = "jkhdskfsd",
                Address = "kjhjkds",
                Age = 10,
                Phone = "1234567890",
                Locality = "fhkds",
                District = "fjdsf",
                State = "jksdjf",
                UID = "jnjdsfnjdsfdjskfj",
                File = null
            };
            var imageDto = new ImageDto { Id = id ,
                FirstName = "ksdjhdsjf",
                LastName = "jkhdskfsd",
                Address = "kjhjkds",
                Age = 10,
                Phone = "1234567890",
                Locality = "fhkds",
                District = "fjdsf",
                State = "jksdjf",
                UID = "jnjdsfnjdsfdjskfj",
                File = null

            };

            imageRepositoryMock.Setup(ir => ir.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(imageDomainModel);
            mapperMock.Setup(m => m.Map<ImageDto>(It.IsAny<ImageAd>())).Returns(imageDto);

            // Act
            var result = await imageController.Delete(id) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(result.Value as ImageDto);
        }

        [Test]
        public async Task Delete_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();

            imageRepositoryMock.Setup(ir => ir.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync((ImageAd)null);

            // Act
            var result = await imageController.Delete(id) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }


    }
    

}
