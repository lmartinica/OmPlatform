﻿using System.ComponentModel.DataAnnotations;

namespace OmPlatform.DTOs.Product
{
    public class CreateProductDto
    {
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public string? Category { get; set; }
    }
}
