﻿namespace OmPlatform.DTOs.User
{
    public class GetUserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}