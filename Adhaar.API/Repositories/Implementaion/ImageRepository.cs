using Adhaar.API.Data;
using Adhaar.API.Models.Domain;
using Adhaar.API.Repositories.Interface;
using Azure.Core;
using IronOcr;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace Adhaar.API.Repositories.Implementaion
{
    [ExcludeFromCodeCoverage]
    public class ImageRepository : IImageRepository
    {
        private readonly AdhaarApiDbContext dbContext;
        private readonly ILogger<ImageRepository> logger;
        private readonly string[] exts;
        private const string folderName = "Images/";
        


        public ImageRepository(AdhaarApiDbContext dbContext,ILogger<ImageRepository> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            exts = new string[3] { ".jpeg", ".jpg", ".png" };
        }
        public async Task<ImageAd> CreateAsync(ImageAd Image)
        {
            await dbContext.Images.AddAsync(Image);
            await dbContext.SaveChangesAsync();
            return Image;
        }

        public async Task<ImageAd?> DeleteAsync(Guid id)
        {
            var existingImage = await dbContext.Images.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (existingImage == null) { return null; }

            dbContext.Images.Remove(existingImage);
            await dbContext.SaveChangesAsync();
            return existingImage;
        }

        public async Task<List<ImageAd>> GetAllAsync()
        {
            return await dbContext.Images.ToListAsync();
        }

        [ExcludeFromCodeCoverage]
        public async Task<ImageAd?> GetByIdAsync(Guid id)
        {
            return await dbContext.Images.FirstOrDefaultAsync(x => (x.Id).Equals(id));
        }

        public async Task<ImageAd?> UpdateAsync(Guid id, ImageAd Image)
        {
            var existingImage = await dbContext.Images.FirstOrDefaultAsync(x => (x.Id).Equals(id));
            if (existingImage == null) { return null; }

            existingImage.Id = id;

            existingImage.State=Image.State==null?existingImage.State:Image.State;

            existingImage.UID = Image.UID==null?existingImage.UID:Image.UID;

            existingImage.FirstName=Image.FirstName==null?existingImage.FirstName:Image.FirstName;

            existingImage.LastName = Image.LastName == null ? existingImage.LastName : Image.LastName;

            existingImage.Address = Image.Address == null ? existingImage.Address : Image.Address;

            existingImage.Age=Image.Age==null?existingImage.Age:Image.Age;

            existingImage.District=Image.District==null?existingImage.District:Image.District;

            existingImage.File=Image.File==null?existingImage.File:Image.File;

            existingImage.Locality=Image.Locality==null?existingImage.Locality:Image.Locality;

            existingImage.Phone=Image.Phone==null?existingImage.Phone:Image.Phone;
            

            await dbContext.SaveChangesAsync();

            return existingImage;

        }

        public async Task<string?> OCR(ImageAd request)
        {
            try
            {
                // Validate if the request contains a file
                if (request?.File == null || request.File.Length == 0)
                {
                    logger.LogInformation("No file uploaded.");
                    return null;
                }

                string fileName = request.File.FileName;
                string filePath = Path.Combine(folderName, fileName);
                string ext = Path.GetExtension(filePath);
                if (!exts.Contains(ext))
                {
                    logger.LogInformation("Unsupported File format");
                    return null;
                }



                // Save the uploaded file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.File.CopyToAsync(fileStream);
                }

                // Validate license
                bool isValidLicense = License.IsValidLicense("IRONSUITE.SUNUGUNUS.GMAIL.COM.17184-E3AE17ABDE-AYWB6KC-2WB3DTPND6ZB-6ZUMCAKUWFW3-WUNYOAQLMMJT-YAMXVLVEP36O-VGU2ZJ54UPU2-JX6PVTXS65HP-IMVX6O-TE5YV4XMPPGLUA-DEPLOYMENT.TRIAL-ACBH3K.TRIAL.EXPIRES.11.FEB.2024");

                if (!isValidLicense)
                {
                    logger.LogInformation("Invalid license.");
                    return null;
                }

                // Setup OCR
                var ocr = new IronTesseract();
               /* ocr.AddSecondaryLanguage(OcrLanguage.Hindi);*/

                // OCR Processing
                using (var ocrInput = new OcrInput())
                {
                    ocrInput.AddImage(filePath);
                    ocrInput.EnhanceResolution();
                    ocrInput.DeNoise();

                    var ocrResult = ocr.Read(ocrInput);

                    //if details match then add the image domain model to the user and return Ok(Image)

                    return ocrResult.Text;
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);
                return null;
            }
        }
    }
}
