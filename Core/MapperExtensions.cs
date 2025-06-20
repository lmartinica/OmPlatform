﻿using OmPlatform.DTOs.Order;
using OmPlatform.DTOs.OrderItems;
using OmPlatform.DTOs.Product;
using OmPlatform.DTOs.User;
using OmPlatform.Models;

namespace OmPlatform.Core
{
    public static class MapperExtensions
    {
        #region Product

        public static GetProductDto ToProductDto(this Products entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            Stock = entity.Stock,
            Category = entity.Category
        };

        public static Products ToProduct(this CreateProductDto dto) => new()
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock,
            Category = dto.Category
        };

        public static void UpdateProduct(this UpdateProductDto dto, Products entity)
        {
            if (dto.Name != null) entity.Name = dto.Name;
            if (dto.Description != null) entity.Description = dto.Description;
            if (dto.Price.HasValue) entity.Price = dto.Price.Value;
            if (dto.Stock.HasValue) entity.Stock = dto.Stock.Value;
            if (dto.Category != null) entity.Category = dto.Category;
        }

        #endregion

        #region User

        public static GetUserDto ToUserDto(this Users entity) => new()
        {
            Id = entity.Id,
            Email = entity.Email,
            Name = entity.Name,
            Role = entity.Role
        };

        public static Users ToUser(this CreateUserDto dto) => new()
        {
            Email = dto.Email,
            Name = dto.Name,
            Password = dto.Password,
        };

        public static void UpdateUser(this UpdateUserDto dto, Users entity)
        {
            if (dto.Name != null) entity.Name = dto.Name;
        }

        #endregion

        #region Order

        public static GetOrderDto ToOrderDto(this Orders entity) => new()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            TotalPrice = entity.TotalPrice,
            Status = entity.Status,
            Created = entity.Created,
            OrderItems = entity.OrderItems.Select(i => new GetOrderItemDto
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList()
        };

        public static Orders ToOrder(this CreateOrderDto dto) => new()
        {
            OrderItems = dto.OrderItems.Select(i => new OrderItems
            {
                ProductId = i.ProductId.Value,
                Quantity = i.Quantity.Value
            }).ToList()
        };

        public static void UpdateOrder(this UpdateOrderDto dto, Orders entity)
        {
            if (dto.Status != null) entity.Status = dto.Status;
        }

        #endregion
    }
}
