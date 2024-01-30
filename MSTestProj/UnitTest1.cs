using Adhaar.API.Data;
using Adhaar.API.Models.Domain;
using Adhaar.API.Repositories.Implementaion;
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.AspNetCore.Http;

namespace MSTestProj
{
    /*[TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }
    }*/
    [TestClass]
    public class ImageRepositoryTests
    {
        private AdhaarApiDbContext dbContext;
        private ILogger<ImageRepository> logger;
        private ImageRepository imageRepository;

        [TestInitialize]
        public void Initialize()
        {
            // Mock DbContext
            var dbContextMock = new Mock<AdhaarApiDbContext>();
            // Add setup for any DbSet methods you might use in your repository

            // Mock ILogger
            var loggerMock = new Mock<ILogger<ImageRepository>>();

            dbContext = dbContextMock.Object;
            logger = loggerMock.Object;

            imageRepository = new ImageRepository(dbContext, logger);
        }

        [TestMethod]
        public async Task CreateAsync_ValidImage_ReturnsImage()
        {
            byte[] fileContent = new byte[] { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100 };
            // Arrange
            var image = new ImageAd
            {
                Id = Guid.NewGuid(),
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

                        baseStream: new MemoryStream(fileContent),
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

            // Act
            var result = await imageRepository.CreateAsync(image);

            // Assert
            Assert.IsNotNull(result);
            // Add more assertions as needed
        }
    }

}