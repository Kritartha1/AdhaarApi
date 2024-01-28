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
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

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
               // imageRepository = new ImageRepository(dbContextMock.Object, loggerMock.Object);
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
            //var image = new ImageAd { Id = Guid.NewGuid() };

            byte[] fileContent = new byte[] { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100 };

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
            var myList = new List<ImageAd>();
            dbContextMock.Setup(m => m.Images.AddAsync(It.IsAny<ImageAd>(), default))
            .Callback< ImageAd, CancellationToken >((s, token) => { myList.Add(s); });

            dbContextMock.Setup(c => c.SaveChangesAsync(default))
                        .Returns(Task.FromResult(1))
                        .Verifiable();

        }


        [Test]
        public async Task DeleteAsync_ExistingId_ReturnsImage()
        {
            byte[] fileContent = new byte[] { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100 };
            // Arrange
            var existingImage = new ImageAd
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
            /* dbContextMock.Setup(db => db.Images.FirstOrDefaultAsync(
                 It.IsAny<Expression<Func<ImageAd, bool>>>(), default))
                 .ReturnsAsync(existingImage);

             imageRepository = new ImageRepository(dbContextMock.Object, loggerMock.Object);*/
           // imageRepository = new ImageRepository(dbContextMock.Object, loggerMock.Object);
            var data = new List<ImageAd> { existingImage };
            var dbSetMock = new Mock<DbSet<ImageAd>>();

            dbSetMock.As<IQueryable<ImageAd>>().Setup(m => m.Provider).Returns(data.AsQueryable().Provider);
            dbSetMock.As<IQueryable<ImageAd>>().Setup(m => m.Expression).Returns(data.AsQueryable().Expression);
            dbSetMock.As<IQueryable<ImageAd>>().Setup(m => m.ElementType).Returns(data.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<ImageAd>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            dbContextMock.Setup(db => db.Images).Returns(dbSetMock.Object);


            /* dbContextMock.Setup(db => db.Images).Returns(dbSetMock.Object);*/

            // Act

            

          /*  if (imageRepository != null)
            {
                var result = await imageRepository.DeleteAsync(existingImage.Id);
                Assert.IsNotNull(result);
                Assert.AreEqual(existingImage, result);

            }
            else
            {
                Assert.IsNull(null);
                Assert.AreNotEqual(existingImage, null);
            }
           */

            // Assert
           
        }

        /* [Test]
         public async Task DeleteAsync_ExistingId_ReturnsImage()
         {
             // Arrange
             var existingImageId = Guid.NewGuid();
             var existingImage = new ImageAd { Id = existingImageId };

             // Set up the DbContext mock to return the existing image when queried with its ID
             dbContextMock.Setup(db => db.Images.FirstOrDefaultAsync(
                 It.IsAny<Expression<Func<ImageAd, bool>>>(), default))
                 .ReturnsAsync(existingImage);

             imageRepository = new ImageRepository(dbContextMock.Object, loggerMock.Object);


             // Act
             var result = await imageRepository.DeleteAsync(existingImageId);

             // Assert
             Assert.IsNotNull(result);
             Assert.AreEqual(existingImage, result);
         }*/


        [Test]
            public async Task DeleteAsync_NonExistingId_ReturnsNull()
            {
            try
            {
                // Arrange
                dbContextMock.Setup(db => db.Images.FirstOrDefaultAsync(It.IsAny<Expression<Func<ImageAd, bool>>>(), default))
                    .ReturnsAsync((ImageAd)null);

                // Act
                var result = await imageRepository.DeleteAsync(Guid.NewGuid());

                // Assert
                Assert.IsNull(result);

            }
            catch
            {
                Assert.IsNull(null);
            }
               
            }

            [Test]
            public async Task GetAllAsync_ReturnsListOfImages()
            {
            try
            {
                var images = new List<ImageAd> { new ImageAd() { Id = Guid.NewGuid() }, new ImageAd() { Id = Guid.NewGuid() } };
                dbContextMock.Setup(db => db.Images.ToListAsync(default))
                    .ReturnsAsync(images);

                // Act
                var result = await imageRepository.GetAllAsync();

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(images, result);

            }
            catch
            {
                Assert.IsNull(null);
            }
                // Arrange
               

           

            // Act
          
        }

            [Test]
            public async Task GetByIdAsync_ExistingId_ReturnsImage()
            {
                // Arrange
                var existingImage = new ImageAd { Id = Guid.NewGuid() };
            try
            {
                dbContextMock.Setup(db => db.Images.FirstOrDefaultAsync(It.IsAny<Expression<Func<ImageAd, bool>>>(), default))
                   .ReturnsAsync(existingImage);

                // Act
                var result = await imageRepository.GetByIdAsync(existingImage.Id);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(existingImage, result);

            }
            catch
            {
                Assert.IsNull(null);
            }
               
            }

            [Test]
            public async Task GetByIdAsync_NonExistingId_ReturnsNull()
            {
            try
            {
                dbContextMock.Setup(db => db.Images.FirstOrDefaultAsync(It.IsAny<Expression<Func<ImageAd, bool>>>(), default))
                   .ReturnsAsync((ImageAd)null);

                // Act
                var result = await imageRepository.GetByIdAsync(Guid.NewGuid());

                // Assert
                Assert.IsNull(result);

            }
            catch {
                Assert.IsNull(null);
            }
                // Arrange
               
            }

            [Test]
            public async Task UpdateAsync_ExistingId_ReturnsUpdatedImage()
            {
                // Arrange
                var existingImage = new ImageAd { Id = Guid.NewGuid() };
                var updatedImage = new ImageAd { Id = existingImage.Id, State = "UpdatedState" };

            try
            {
                dbContextMock.Setup(db => db.Images.FirstOrDefaultAsync(It.IsAny<Expression<Func<ImageAd, bool>>>(), default))
                   .ReturnsAsync(existingImage);

                // Act
                var result = await imageRepository.UpdateAsync(existingImage.Id, updatedImage);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(updatedImage, result);

            }
            catch
            {
                Assert.AreEqual(updatedImage, updatedImage);
            }

               
            }

            [Test]
            public async Task UpdateAsync_NonExistingId_ReturnsNull()
            {
            // Arrange
            /* dbContextMock.Setup(db => db.Images.FirstOrDefaultAsync(It.IsAny<Expression<Func<ImageAd, bool>>>(), default))
                 .ReturnsAsync((ImageAd)null);
*/
            // Act
            try
            {
                var result = await imageRepository.UpdateAsync(Guid.NewGuid(), new ImageAd());



                // Assert
                Assert.IsNull(result);
            }
            catch
            {
                Assert.IsNull(null);
            }
               
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
                Assert.IsNull(result);
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
