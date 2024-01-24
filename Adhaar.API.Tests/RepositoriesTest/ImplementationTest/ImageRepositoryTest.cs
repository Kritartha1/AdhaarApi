using Adhaar.API.Data;
using Adhaar.API.Models.Domain;
using global::Adhaar.API.Repositories.Implementaion;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Adhaar.API.Data;
using Adhaar.API.Models.Domain;
using Adhaar.API.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Linq.Expressions;

namespace Adhaar.API.Tests.RepositoriesTest.ImplementationTest
{



    [TestFixture]
        public class ImageRepositoryTests
        {
            private ImageRepository imageRepository;
            private Mock<AdhaarApiDbContext> dbContextMock;
            private Mock<ILogger<ImageRepository>> loggerMock;

            [SetUp]
            public void Setup()
            {
                dbContextMock = new Mock<AdhaarApiDbContext>();
                loggerMock = new Mock<ILogger<ImageRepository>>();
                imageRepository = new ImageRepository(null, loggerMock.Object);
            }

        /*  [Test]
          public async Task CreateAsync_ValidImage_ReturnsImage()
          {
              // Arrange
              var image = new ImageAd { Id = Guid.NewGuid() };


              dbContextMock.Setup(db => db.Images.AddAsync(It.IsAny<ImageAd>(), default))
              .ReturnsAsync((ImageAd imageAd, CancellationToken _) =>
              {
                  var entry = dbContextMock.Object.Entry(imageAd);
                  return entry;
              });


          // Act
          var result = await imageRepository.CreateAsync(image);

              // Assert
              Assert.IsNotNull(result);
              Assert.AreEqual(image, result);
          }*/


        [Test]
        public async Task CreateAsync_ValidImage_ReturnsImage()
        {
            // Arrange
            var image = new ImageAd { Id = Guid.NewGuid() };



            // Set up AddAsync

            dbContextMock.Setup(db => db.Images.AddAsync(It.IsAny<ImageAd>(), default))
                .ReturnsAsync((ImageAd imageAd, CancellationToken _) =>
                {
                    // Ensure dbContextMock.Object is not null
                    if (dbContextMock.Object != null)
                    {
                        var entry = dbContextMock.Object.Entry(imageAd);
                        return entry;
                    }

                    // Return a default value if dbContextMock.Object is null
                    return null;
                });

            // Act
            var result = await imageRepository.CreateAsync(image);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(image, result);
        }




        [Test]
            public async Task DeleteAsync_ExistingId_ReturnsImage()
            {
                // Arrange
                var existingImage = new ImageAd { Id = Guid.NewGuid() };
                dbContextMock.Setup(db => db.Images.FirstOrDefaultAsync(It.IsAny<Expression<Func<ImageAd, bool>>>(), default))
                    .ReturnsAsync(existingImage);

                // Act
                var result = await imageRepository.DeleteAsync(existingImage.Id);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(existingImage, result);
            }

            [Test]
            public async Task DeleteAsync_NonExistingId_ReturnsNull()
            {
                // Arrange
                dbContextMock.Setup(db => db.Images.FirstOrDefaultAsync(It.IsAny<Expression<Func<ImageAd, bool>>>(), default))
                    .ReturnsAsync((ImageAd)null);

                // Act
                var result = await imageRepository.DeleteAsync(Guid.NewGuid());

                // Assert
                Assert.IsNull(result);
            }

            [Test]
            public async Task GetAllAsync_ReturnsListOfImages()
            {
                // Arrange
                var images = new List<ImageAd> { new ImageAd(), new ImageAd() };
                dbContextMock.Setup(db => db.Images.ToListAsync(default))
                    .ReturnsAsync(images);

                // Act
                var result = await imageRepository.GetAllAsync();

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(images, result);
            }

            [Test]
            public async Task GetByIdAsync_ExistingId_ReturnsImage()
            {
                // Arrange
                var existingImage = new ImageAd { Id = Guid.NewGuid() };
                dbContextMock.Setup(db => db.Images.FirstOrDefaultAsync(It.IsAny<Expression<Func<ImageAd, bool>>>(), default))
                    .ReturnsAsync(existingImage);

                // Act
                var result = await imageRepository.GetByIdAsync(existingImage.Id);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(existingImage, result);
            }

            [Test]
            public async Task GetByIdAsync_NonExistingId_ReturnsNull()
            {
                // Arrange
                dbContextMock.Setup(db => db.Images.FirstOrDefaultAsync(It.IsAny<Expression<Func<ImageAd, bool>>>(), default))
                    .ReturnsAsync((ImageAd)null);

                // Act
                var result = await imageRepository.GetByIdAsync(Guid.NewGuid());

                // Assert
                Assert.IsNull(result);
            }

            [Test]
            public async Task UpdateAsync_ExistingId_ReturnsUpdatedImage()
            {
                // Arrange
                var existingImage = new ImageAd { Id = Guid.NewGuid() };
                var updatedImage = new ImageAd { Id = existingImage.Id, State = "UpdatedState" };

                dbContextMock.Setup(db => db.Images.FirstOrDefaultAsync(It.IsAny<Expression<Func<ImageAd, bool>>>(), default))
                    .ReturnsAsync(existingImage);

                // Act
                var result = await imageRepository.UpdateAsync(existingImage.Id, updatedImage);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(updatedImage, result);
            }

            [Test]
            public async Task UpdateAsync_NonExistingId_ReturnsNull()
            {
                // Arrange
                dbContextMock.Setup(db => db.Images.FirstOrDefaultAsync(It.IsAny<Expression<Func<ImageAd, bool>>>(), default))
                    .ReturnsAsync((ImageAd)null);

                // Act
                var result = await imageRepository.UpdateAsync(Guid.NewGuid(), new ImageAd());

                // Assert
                Assert.IsNull(result);
            }

            [Test]
            public async Task OCR_ValidImage_ReturnsOcrText()
            {
                // Arrange
                var image = new ImageAd
                {
                    File = new FormFile(Stream.Null, 0, 0, "file", "test.jpg")
                };




            dbContextMock.Setup(db => db.Images.AddAsync(It.IsAny<ImageAd>(), default))
            .ReturnsAsync((ImageAd imageAd, CancellationToken _) =>
            {
                var entry = dbContextMock.Object.Entry(imageAd);
                return entry;
            });


            // Act
            var result = await imageRepository.OCR(image);

                // Assert
                Assert.IsNotNull(result);
                // Add further assertions based on OCR logic
            }

            [Test]
            public async Task OCR_InvalidImage_ReturnsNull()
            {
                // Arrange
                var image = new ImageAd(); // No file attached

                // Act
                var result = await imageRepository.OCR(image);

                // Assert
                Assert.IsNull(result);
            }
        }
    

}
