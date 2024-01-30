

using Adhaar.API.Data;
using Adhaar.API.Models.Domain;
using Adhaar.API.Models.DTO;
using Adhaar.API.Repositories.Interface;
using AutoMapper;
using IronOcr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IronOcr;
using Tesseract;
using Microsoft.VisualBasic.FileIO;
using System.IO.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace Adhaar.API.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;
        private readonly IMapper mapper;
        private readonly AdhaarApiDbContext dbContext;
        private readonly IUserRepository userRepository;
        private readonly string[] exts;
        private const string folderName = "Images/";
        /* private readonly IFileSystem fileSystem;*/

        /* public AuthController(ILogger<AuthController> logger,UserManager<IdentityUser> userManager, ITokenRepository tokenRepository, IMapper mapper, AdhaarApiDbContext dbContext, IUserRepository userRepository,IFileSystem fileSystem)
         {
             this.logger = logger;
             this.userManager = userManager;
             this.tokenRepository = tokenRepository;
             this.mapper = mapper;
             this.dbContext = dbContext;
             this.userRepository = userRepository;
             exts =new string[3]{ ".jpeg",".jpg",".png"};
             this.fileSystem = fileSystem;

         }*/
        public AuthController(ILogger<AuthController> logger, UserManager<IdentityUser> userManager, ITokenRepository tokenRepository, IMapper mapper, AdhaarApiDbContext dbContext, IUserRepository userRepository)
        {
            this.logger = logger;
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.userRepository = userRepository;
            exts = new string[3] { ".jpeg", ".jpg", ".png" };
           

        }

        //Post :  /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto registerRequestDto)
        {

            var identityUser = new User
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };

            //var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);
           
            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);


            if (identityResult.Succeeded)
            {

                //Add roles to this User
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                    if (identityResult.Succeeded)
                    {
                        var identity_user = await userRepository.CreateAsync(identityUser);
                        if (identity_user == null)
                        {

                            await userManager.DeleteAsync(identityUser);

                            return BadRequest("Oops! something went wrong!");
                        }

                        var userDTO = mapper.Map<UserDto>(identity_user);
                        return CreatedAtAction(nameof(GetById), new { id = userDTO.Id }, userDTO);

                        
                    }
                }
            }

            await userRepository.DeleteAsync(identityUser.Id);
            await userManager.DeleteAsync(identityUser);

            return BadRequest("Oops! something went wrong!");

        }


        //Post :  /api/Auth/Login

        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);
            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (checkPasswordResult)
                {
                    //Get the roles of user
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
                        
                     
                        var response = new LoginResponseDto
                        {
                            Email = loginRequestDto.Username,
                            Roles = roles.ToList(),
                            JwtToken = jwtToken,
                            Id = user.Id
                        };
                        return Ok(response);

                    }
                    //create token
                    return BadRequest("No roles provided!");
                }
                return BadRequest("Wrong password!");
            }
            return BadRequest("wrong username or password");
        }


        [ExcludeFromCodeCoverage]
        [HttpPost]
        [Route("OCR/{id}")]
        public async Task<IActionResult> DoOCR([FromRoute] string id,[FromForm] ImageAd request)
        {
            //here id is user id
            try
            {
                // Validate if the request contains a file
                if (request?.File == null || request.File.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }

                string fileName = request.File.FileName;
                string filePath = Path.Combine(folderName, fileName);
               string ext = Path.GetExtension(filePath);
                if (!exts.Contains(ext))
                {
                    return BadRequest("Unsupported File format.");
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
                    return BadRequest("Invalid license.");
                }

                // Setup OCR
                var ocr = new IronTesseract();

                // OCR Processing
                using (var ocrInput = new OcrInput())
                {
                    ocrInput.AddImage(filePath);
                    ocrInput.EnhanceResolution();
                    ocrInput.DeNoise();

                    var ocrResult = ocr.Read(ocrInput);

                    //if details match then add the image domain model to the user and return Ok(Image)
                    
                    return Ok(ocrResult.Text);
                }
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        [HttpGet]
        [Route("{id}")]
        //[Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {

            var userDomain = await userRepository.GetByIdAsync(id);

            if (userDomain == null)
            {
                return NotFound();
            }

            var userDto = mapper.Map<UserDto>(userDomain);

          
              return Ok(userDto);


        }

        [NonAction]

        public void SetFileSystem(System.IO.Abstractions.IFileSystem @object)
        {
            throw new NotImplementedException();
        }
    }
}

