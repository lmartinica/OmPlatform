﻿using System.ComponentModel.DataAnnotations;

namespace OmPlatform.DTOs.OrderItems
{
    public class CreateOrderItemDto
    {
        [Required]
        public Guid? ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Product quantity must be at least 1.")]
        public int? Quantity { get; set; }
    }
}