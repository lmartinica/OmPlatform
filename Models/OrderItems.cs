﻿namespace OmPlatform.Models
{
    public class OrderItems
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public Orders Order { get; set; }
        public Products Product { get; set; }

    }
}
