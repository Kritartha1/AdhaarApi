using Adhaar.API.SMS;

namespace Adhaar.API.Services
{
    public interface IIdentityMessageService
    {
        public Task<bool> SendAsync(MessageRequest message);
    }
}