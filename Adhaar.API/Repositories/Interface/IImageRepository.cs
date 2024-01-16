using Adhaar.API.Models.Domain;

namespace Adhaar.API.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<List<ImageAd>> GetAllAsync();

        Task<ImageAd?> GetByIdAsync(Guid id);

        Task<ImageAd> CreateAsync(ImageAd Image);

        Task<ImageAd?> UpdateAsync(Guid id, ImageAd Image);

        Task<ImageAd?> DeleteAsync(Guid id);

        Task<string?> OCR(ImageAd request);
    }
}
