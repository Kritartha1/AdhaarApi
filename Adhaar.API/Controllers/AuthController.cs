

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

namespace Adhaar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {


        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;
        private readonly IMapper mapper;
        private readonly AdhaarApiDbContext dbContext;
        private readonly IUserRepository userRepository;
       

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository, IMapper mapper, AdhaarApiDbContext dbContext, IUserRepository userRepository)
        {

            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.userRepository = userRepository;
           
        }

        [HttpGet]
        [Route("{id}")]
       //[Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {

            var userDomain = await userRepository.GetByIdAsync(id);

            if (userDomain == null )
            {
                return NotFound();
            }

            var userDto = mapper.Map<UserDto>(userDomain);

            bool result = License.IsValidLicense("IRONSUITE.SUNUGUNUS.GMAIL.COM.17184-E3AE17ABDE-AYWB6KC-2WB3DTPND6ZB-6ZUMCAKUWFW3-WUNYOAQLMMJT-YAMXVLVEP36O-VGU2ZJ54UPU2-JX6PVTXS65HP-IMVX6O-TE5YV4XMPPGLUA-DEPLOYMENT.TRIAL-ACBH3K.TRIAL.EXPIRES.11.FEB.2024");

            var ocr = new IronTesseract();

            using (var ocrInput = new OcrInput())
            {
                ocrInput.AddImage(@"Images\HAHA.png");
                ocrInput.AddImage(@"Images\ab.png");
          
                ocrInput.EnhanceResolution();
                ocrInput.DeNoise();

                // Optionally Apply Filters if needed:
                // ocrInput.Deskew();  // use only if image not straight
                // ocrInput.DeNoise(); // use only if image contains digital noise

                var ocrResult = ocr.Read(ocrInput);
                return Ok(ocrResult.Text);
            }

            return BadRequest();
          //  return Ok(userDto);

        
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
                    return Ok();
                }
                return Ok(loginRequestDto.Username);
            }
            return BadRequest("wrong username or password");
        }



        [HttpPost]
        [Route("Verify")]


        public async Task<IActionResult> Verify([FromBody] RegisterUserRequestDto registerRequestDto)
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
    }
}

