﻿using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Dtos
{
    public class SignUpDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string City { get; set; }
    }
}
