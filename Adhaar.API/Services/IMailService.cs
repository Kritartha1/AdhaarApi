using Adhaar.API.Helper;
using Adhaar.API.Models.Domain;

namespace Adhaar.API.Services
{
    public interface IMailService
    {
       
        Task<bool> SendMailAsync(MailRequest mailData);
    }
}
