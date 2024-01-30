using Adhaar.API.Data;
using Adhaar.API.Models.Domain;
using Adhaar.API.Models.DTO;
using Adhaar.API.Repositories.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp.Drawing;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Adhaar.API.Controllers
{
   // [ExcludeFromCodeCoverage]
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly AdhaarApiDbContext dbContext;
        private readonly IUserRepository userRepository;
        private readonly IImageRepository imageRepository;
        private readonly ILogger<ImageController> logger1;

        public ImageController(IMapper mapper, AdhaarApiDbContext dbContext, IUserRepository userRepository,IImageRepository imageRepository,ILogger<ImageController> logger1)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.userRepository = userRepository;
            this.imageRepository = imageRepository;
            this.logger1 = logger1;
        }

        [HttpGet]
        //  [Authorize(Roles ="User")]

        public async Task<IActionResult> GetAll()
        {
            logger1.LogInformation("GetAll users Action method was invoked");

            var imagesDomain = await imageRepository.GetAllAsync();
            if (imagesDomain == null)
            {
                return NotFound("No users found");
            }

  

            logger1.LogInformation($"Finished GetAllUsers request with data:{JsonSerializer.Serialize(imagesDomain)}");

            var imagesDto = mapper.Map<List<ImageDto>>(imagesDomain);

            return Ok(imagesDto);

        }


       

        [HttpGet]
        [Route("{id}")]
        /* [Authorize(Roles = "User,Admin")]*/
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var userDomain=await userRepository.GetByIdAsync(id);
            if(userDomain == null)
            {
                return NotFound("No user found!");
            }



            var imageDomain = userDomain.Image;
            if (imageDomain == null)
            {
                return NotFound("Not verified");
            }
            var imageDto = mapper.Map<ImageDto>(imageDomain);


            return Ok(imageDto);

        }


        /// ////////////////////////////////////////////////////////////////////////////////////   

        [HttpPost]
        [Route("{id}")]
        //[Authorize(Roles = "User")]

        public async Task<IActionResult> Create([FromRoute] string id,[FromForm] AddImageRequestDto addImageRequestDto)
        {
          
            var imageDomainModel = mapper.Map<ImageAd>(addImageRequestDto);

            imageDomainModel = await imageRepository.CreateAsync(imageDomainModel);

            

            if(imageDomainModel == null)
            {
                return BadRequest("Oops! Something is wrong. Try again!");
            }

            logger1.LogInformation($"Id of image is : {imageDomainModel.Id}");

            var user = userRepository.GetByIdAsync(id);
            if (user == null)
            {
                await imageRepository.DeleteAsync(imageDomainModel.Id);
                return BadRequest("No user found");
            }

            string? text = await imageRepository.OCR(imageDomainModel);

            if(text == null)
            {
                await imageRepository.DeleteAsync(imageDomainModel.Id);
                return BadRequest("Oops! Something is wrong in your form. Try again!");

            }
            text = text.ToLower();
           

            string[] result = Regex.Split(text, @"[\s\p{P}]");

            text = Regex.Replace(text, @"\s+", "");

            // Remove empty entries from the resulting array
            result = result.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();



            foreach (string word in result)
            {
                logger1.LogInformation(word);
            }



            logger1.LogInformation(text);



            /*if (text.Contains(imageDomainModel.Address.ToLower())
                &&text.Contains(imageDomainModel.State.ToLower())
                &&text.Contains(imageDomainModel.District.ToLower())
                &&text.Contains(imageDomainModel.FirstName.ToLower())
                &&text.Contains(imageDomainModel.LastName.ToLower())
                &&text.Contains(imageDomainModel.UID)
                )
            {*/

           /*  && result.Contains(imageDomainModel.FirstName.ToLower())
                && result.Contains(imageDomainModel.LastName.ToLower())*/

            if (result.Contains(imageDomainModel.Address.ToLower())
                && result.Contains(imageDomainModel.State.ToLower())
                && result.Contains(imageDomainModel.District.ToLower())
                

                && text.Contains(imageDomainModel.UID)
                )
                {
                    var userDomainModel = await userRepository.UpdateAsync(id,
                        new User
                        {
                            Id = id,
                            ImageId = imageDomainModel.Id,
                            Image = imageDomainModel

                        });
                var userDto = mapper.Map<UserDto>(userDomainModel);
                return Ok(userDto);
            }

            return BadRequest("Details didn't match");
            
           


            /*var imageDTO = mapper.Map<ImageDto>(imageDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = imageDTO.Id }, imageDTO);*/

        }

        /// ////////////////////////////////////////////////////////////////////////////////////  


       /* [HttpPut]
        [Route("{id:Guid}")]
        *//* [Authorize(Roles = "User")]*//*

        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateImageRequestDto updateImageRequestDto)
        {

            var imageDomainModel = mapper.Map<ImageAd>(updateImageRequestDto);
            imageDomainModel = await imageRepository.UpdateAsync(id, imageDomainModel);

            if (imageDomainModel == null)
            {
                return NotFound();
            }

            var imageDto = mapper.Map<ImageDto>(imageDomainModel);
            return Ok(imageDto);

        }*/

        /// ////////////////////////////////////////////////////////////////////////////////////  



        [HttpDelete]
        [Route("{id:Guid}")]
        /*[Authorize(Roles = "Admin,User")]*/

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var imageDomainModel = await imageRepository.DeleteAsync(id);
            if (imageDomainModel == null)
            {
                return NotFound();
            }

            var imageDto = mapper.Map<ImageDto>(imageDomainModel);

            return Ok(imageDto);
        }

       

    }



    
}
