using Adhaar.API.Models.DTO;
using Adhaar.API.Services;
using Adhaar.API.SMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Adhaar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMSController : ControllerBase
    {
        private readonly IIdentityMessageService messageService;

        public SMSController(IIdentityMessageService messageService)
        {
            this.messageService = messageService;
        }

        [HttpPost]
        [Route("sms")]

        public async Task<IActionResult> SendSMS([FromBody]MessageRequest message)
        {
           bool t=await messageService.SendAsync(message);
            if (t)
            {
                return Ok("Message sent successfully");
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Message not sent ");

        }
    }
}
