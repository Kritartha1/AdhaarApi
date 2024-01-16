﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adhaar.API.Models.DTO
{
    public class VerifyUserDto
    {


        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string? Age { get; set; }


        public string Phone { get; set; }

        public string? Locality { get; set; }

        public string? District { get; set; }

        public string State { get; set; }

        public string UID { get; set; }

        public IFormFile File { get; set; }


    }
}