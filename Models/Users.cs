﻿namespace OmPlatform.Models
{
    public class Users
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public ICollection<Orders> Orders { get; set; }
    }
}