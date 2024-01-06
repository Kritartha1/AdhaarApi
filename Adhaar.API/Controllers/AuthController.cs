

using Adhaar.API.Data;
using Adhaar.API.Models.Domain;
using Adhaar.API.Models.DTO;
using Adhaar.API.Repositories.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        [Authorize(Roles = "User,Doctor")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {

            var userDomain = await userRepository.GetByIdAsync(id);

            if (userDomain == null )
            {
                return NotFound();
            }

            var userDto = mapper.Map<UserDto>(userDomain);
            return Ok(userDto);

        
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
    }
}

