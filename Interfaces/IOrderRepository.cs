﻿using OmPlatform.Models;

namespace OmPlatform.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Orders>> GetList();
        Task<Orders?> GetById(Guid id);
        Task<Orders> Create(Orders order);
        Task Delete(Orders order);
    }
}
