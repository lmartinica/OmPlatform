﻿namespace OmPlatform.DTOs.OrderItems
{
    public class CreateOrderItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
