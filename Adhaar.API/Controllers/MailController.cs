using Adhaar.API.Helper;
using Adhaar.API.Models.Domain;
using Adhaar.API.Repositories.Interface;
using Adhaar.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using static IronOcr.OcrResult;

namespace Adhaar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;
        //injecting the IMailService into the constructor
        public MailController(IMailService _MailService)
        {
           this._mailService = _MailService;
        }

       

        [HttpPost]
        [Route("SendMailAsync")]
        public async  Task<IActionResult> SendMailAsync(MailRequest mailData)
        {
            bool t=await _mailService.SendMailAsync(mailData);
            if (t)
            {
                return Ok("Mail sent successfully!");
            }
            else
            {
                return BadRequest();
            }
           
        }

        /*private string GetHtmlcontent()
        {
            string Response = "<div style=\"width:100%;background-color:lightblue;text-align:center;margin:10px\">";
            Response += "<h1>Welcome to IdentifyMe</h1>";
            Response += "<img src=\"https://img.icons8.com/color-glass/48/verified-account--v1.png\" />";
            Response += "<h2>Thanks!</h2>";
            Response += "</div>";
            return Response;
        }*/

    }
}
