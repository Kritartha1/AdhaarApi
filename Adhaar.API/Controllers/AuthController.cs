/*

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
        private readonly AppointmentApiDbContext dbContext;
        private readonly IUserRepository userRepository;
        private readonly IDoctorRepository doctorRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository, IMapper mapper, AppointmentApiDbContext dbContext, IUserRepository userRepository, IDoctorRepository doctorRepository)
        {

            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.userRepository = userRepository;
            this.doctorRepository = doctorRepository;
        }

        [HttpGet]
        [Route("{id}")]
        *//*[Authorize(Roles = "User,Doctor")]*//*
        public async Task<IActionResult> GetById([FromRoute] string id)
        {

            var userDomain = await userRepository.GetByIdAsync(id);
            var doctorDomain = await doctorRepository.GetByIdAsync(id);
            if (userDomain == null && doctorDomain == null)
            {
                return NotFound();
            }

            *//* var regionsDto = new List<RegionDto>();

             var regionDto = new RegionDto
             {
                 Id = regionDomain.Id,
                 Code = regionDomain.Code,
                 Name = regionDomain.Name,
                 RegionNameUrl = regionDomain.RegionNameUrl,
             };*//*
            if (userDomain == null)
            {
                var userDto = mapper.Map<UserDto>(userDomain);
                return Ok(userDto);

            }
            else
            {
                var doctorDto = mapper.Map<DoctorDto>(doctorDomain);
                return Ok(doctorDto);
            }






        }
        [HttpPost]
        [Route("RegisterUser")]


        public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto registerRequestDto)
        {





            var identityUser = new User
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username,
                Name = registerRequestDto.Name,
                Age = registerRequestDto.Age,
                DOB = registerRequestDto.DOB,
                Address = registerRequestDto.Address,
                AddressId = registerRequestDto.AddressId,

            };
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

        [HttpPost]
        [Route("RegisterDoctor")]


        public async Task<IActionResult> Register([FromBody] RegisterDoctorRequestDto registerRequestDto)
        {


            var identityUser = new Doctor
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username,
                Specialization = registerRequestDto.Specialization,
                Qualifications = registerRequestDto.Qualifications,
                Name = registerRequestDto.Name,
                Hospital = registerRequestDto.Hospital,
                Fees = registerRequestDto.Fees

            };
            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);
            if (identityResult.Succeeded)
            {
                //Add roles to this User
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                    if (identityResult.Succeeded)
                    {
                        var identity_user = await doctorRepository.CreateAsync(identityUser);
                        if (identity_user == null)
                        {

                            await userManager.DeleteAsync(identityUser);

                            return BadRequest("Oops! something went wrong!");
                        }
                        var doctorDTO = mapper.Map<DoctorDto>(identity_user);
                        return CreatedAtAction(nameof(GetById), new { id = doctorDTO.Id }, doctorDTO);

                    }


                }



            }
            await doctorRepository.DeleteAsync(identityUser.Id);
            await userManager.DeleteAsync(identityUser);





            return BadRequest("Oops! something went wrong!");

        }


        //Post :  /api/Auth/Register
        [HttpPost]
        [Route("RegisterAdmin")]


        public async Task<IActionResult> Register([FromBody] RegisterAdminRequestDto registerRequestDto)
        {


            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };
            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);
            if (identityResult.Succeeded)
            {
                //Add roles to this User
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                    if (identityResult.Succeeded)
                    {

                        return Ok("Admin registered");

                    }


                }


            }


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

                    *//*
                                        return Ok();*//*
                }
                *//*return Ok(loginRequestDto.Username);*//*
            }
            return BadRequest("wrong username or password");
        }
    }
}

*/