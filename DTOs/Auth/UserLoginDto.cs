﻿using System.ComponentModel.DataAnnotations;

namespace OmPlatform.DTOs.Auth
{
    public class UserLoginDto
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}