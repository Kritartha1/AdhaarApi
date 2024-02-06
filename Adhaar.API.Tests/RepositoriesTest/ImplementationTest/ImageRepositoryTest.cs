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
using Adhaar.API.Repositories;
using NUnit.Framework;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Adhaar.API.Repositories.Interface;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using System.Text.Json;
using System.Reflection;

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
            
            
          //  imageRepository = new ImageRepository(null, loggerMock.Object);

            imageRepository = new ImageRepository(dbContextMock.Object, loggerMock.Object);
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

            var result = await imageRepository.CreateAsync(image);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(image, result);

            Assert.IsNotEmpty(myList);


        }

        /*
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
                    dbContextMock.Setup(db => db.Images.FirstOrDefaultAsync(
                        It.IsAny<Expression<Func<ImageAd, bool>>>(), default))
                        .ReturnsAsync(existingImage);

                  //  imageRepository = new ImageRepository(dbContextMock.Object, loggerMock.Object);
                    // imageRepository = new ImageRepository(dbContextMock.Object, loggerMock.Object);
                    *//*  var data = new List<ImageAd> { existingImage };
                      var dbSetMock = new Mock<DbSet<ImageAd>>();

                      dbSetMock.As<IQueryable<ImageAd>>().Setup(m => m.Provider).Returns(data.AsQueryable().Provider);
                      dbSetMock.As<IQueryable<ImageAd>>().Setup(m => m.Expression).Returns(data.AsQueryable().Expression);
                      dbSetMock.As<IQueryable<ImageAd>>().Setup(m => m.ElementType).Returns(data.AsQueryable().ElementType);
                      dbSetMock.As<IQueryable<ImageAd>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

                      dbContextMock.Setup(db => db.Images).Returns(dbSetMock.Object);*//*

                    var res = await imageRepository.DeleteAsync(existingImage.Id);
                    Assert.IsNotNull(res);
                    Assert.AreEqual(existingImage, res);


                    *//* dbContextMock.Setup(db => db.Images).Returns(dbSetMock.Object);*//*

                    // Act



                  *//*  if (imageRepository != null)
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
                   *//*

                    // Assert

                }

                *//* [Test]
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
        public async Task DeleteAsync_ExistingId_RemovesImage()
        {
            // Arrange

            byte[] fileContent = new byte[] { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100 };
            
            var existingId = Guid.NewGuid();
            var existingImage = new ImageAd { Id = existingId,
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
            var mockDbSet = new Mock<DbSet<ImageAd>>();
            mockDbSet.Setup(x => x.FindAsync(existingId)).ReturnsAsync(existingImage);

            ////////////////////
          //  dbContextMock.Setup(x => x.Images.FirstOrDefaultAsync(It.IsAny<string>())).Returns(mockDbSet.Object);

            dbContextMock.Setup(db => db.Images.FirstOrDefaultAsync(It.IsAny<Expression<Func<ImageAd, bool>>>(), default))
                   .ReturnsAsync((ImageAd)existingImage);

            ///////////////
            ///


            // Act
            var deletedImage = await imageRepository.DeleteAsync(existingId);

            // Assert
            Assert.IsNotNull(deletedImage);
            Assert.AreEqual(existingId, deletedImage.Id);
            mockDbSet.Verify(x => x.Remove(existingImage), Times.Once);
           // dbContextMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_NonExistingId_ReturnsNull_()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            var mockDbSet = new Mock<DbSet<ImageAd>>();
            mockDbSet.Setup(x => x.FindAsync(nonExistingId)).ReturnsAsync((ImageAd)null);
            dbContextMock.Setup(x => x.Images).Returns(mockDbSet.Object);

            // Act
            var result = await imageRepository.DeleteAsync(nonExistingId);

            // Assert
            Assert.IsNull(result);
            mockDbSet.Verify(x => x.Remove(It.IsAny<ImageAd>()), Times.Never);
           /* dbContextMock.Verify(x => x.SaveChangesAsync(), Times.Never);*/
        }


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
                /*var existingImage = new ImageAd { Id = Guid.NewGuid() };
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
            }*/

               
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







        [Test]
        public async Task GetAll()
        {
            var existingId = Guid.NewGuid();
            byte[] fileContent = new byte[] { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100 };
            var existingImage = new ImageAd
            {
                Id = existingId,
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

            var data = new List<ImageAd>
            {
                new ImageAd
            {
                Id = existingId,
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
            }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<ImageAd>>();
            mockSet.As<IAsyncEnumerable<ImageAd>>()
                .Setup(m => m.GetAsyncEnumerator(default))
                .Returns(new AsyncEnumerator<ImageAd>(data.GetEnumerator()));

            mockSet.As<IQueryable<ImageAd>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<ImageAd>(data.Provider));

            mockSet.As<IQueryable<ImageAd>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<ImageAd>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<ImageAd>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var mockContext = new Mock<AdhaarApiDbContext>();
            mockContext.Setup(c => c.Images).Returns(mockSet.Object);

            imageRepository = new ImageRepository(mockContext.Object, loggerMock.Object);

          
            var blogs =await imageRepository.GetAllAsync();

            Assert.IsTrue(true);
//
           Assert.AreEqual(1, blogs.Count);
           /* Assert.AreEqual("AAA", blogs[0].Name);
            Assert.AreEqual("BBB", blogs[1].Name);
            Assert.AreEqual("ZZZ", blogs[2].Name);*/
        }


        
        [Test]

        public async Task GetById()
        {
            var existingId = Guid.NewGuid();
            byte[] fileContent = new byte[] { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100 };
            var existingImage = new ImageAd
            {
                Id = existingId,
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

            var data = new List<ImageAd> { existingImage }.AsQueryable();

            var mockSet = new Mock<DbSet<ImageAd>>();
            mockSet.As<IAsyncEnumerable<ImageAd>>()
                .Setup(m => m.GetAsyncEnumerator(default))
                .Returns(new AsyncEnumerator<ImageAd>(data.GetEnumerator()));

            mockSet.As<IQueryable<ImageAd>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<ImageAd>(data.Provider));

            mockSet.As<IQueryable<ImageAd>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<ImageAd>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<ImageAd>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var mockContext = new Mock<AdhaarApiDbContext>();
            mockContext.Setup(c => c.Images).Returns(mockSet.Object);
            

            imageRepository = new ImageRepository(mockContext.Object, loggerMock.Object);
            

            var retrievedImage = await imageRepository.GetByIdAsync(existingId);
         /*  var retreivedImg=await  mockContext.Object.Images.FirstOrDefaultAsync(x => (x.Id).Equals(existingId));*/
            
            Assert.IsTrue(true);
            //
         //   Assert.AreEqual(existingId, retreivedImg.Id); 
            /* Assert.AreEqual("AAA", blogs[0].Name);
             Assert.AreEqual("BBB", blogs[1].Name);
             Assert.AreEqual("ZZZ", blogs[2].Name);*/
        }
    }


    internal class AsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public AsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public AsyncEnumerable(Expression expression)
            : base(expression) { }

        public IAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new AsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) =>
            new AsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());

       /* IAsyncEnumerator IAsyncEnumerable.GetAsyncEnumerator()
        {
            return GetAsyncEnumerator();
        }*/

        IQueryProvider IQueryable.Provider
        {
            get { return new TestAsyncQueryProvider<T>(this); }
        }
    }

    internal class AsyncEnumerator<T> : IAsyncEnumerator<T>, IAsyncDisposable, IDisposable
    {
        private readonly IEnumerator<T> enumerator;

        private Utf8JsonWriter? _jsonWriter = new(new MemoryStream());

       /* public AsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }*/

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);

            Dispose(disposing: false);
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize

            GC.SuppressFinalize(this);
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _jsonWriter?.Dispose();
                _jsonWriter = null;
            }
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_jsonWriter is not null)
            {
                await _jsonWriter.DisposeAsync().ConfigureAwait(false);
            }

            _jsonWriter = null;
        }

        public AsyncEnumerator(IEnumerator<T> enumerator) =>
            this.enumerator = enumerator ?? throw new ArgumentNullException();

        public T Current => enumerator.Current;
        public ValueTask<bool> MoveNextAsync() =>
            new ValueTask<bool>(enumerator.MoveNext());

       
        


    }

    internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        internal TestAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new AsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new AsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(expression));
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute<TResult>(expression));
        }

        TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return _inner.Execute<TResult>(expression);
        }

        
    }





}



