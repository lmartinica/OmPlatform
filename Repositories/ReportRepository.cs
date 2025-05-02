using Microsoft.EntityFrameworkCore;
using OmPlatform.Core;
using OmPlatform.DTOs.Reports;
using OmPlatform.Models;

namespace OmPlatform.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly DbAppContext _context;

        public ReportRepository(DbAppContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetSaleMonthDto>> GetSales()
        {
            return await _context.Orders
                .GroupBy(o => new { o.Created.Year, o.Created.Month })
                .Select(g => new GetSaleMonthDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Revenue = g.Sum(x => x.TotalPrice)
                })
                .OrderByDescending(x => x.Year).ThenByDescending(x => x.Month)
                .ToListAsync();
        }

        public async Task<IEnumerable<GetTopProductDto>> GetTopProducts()
        {
            return await _context.OrderItems
                .GroupBy(oi => new { oi.ProductId, oi.Product.Name })
                .Select(g => new GetTopProductDto
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.Name,
                    TotalQuantity = g.Sum(x => x.Quantity),
                    TotalRevenue = g.Sum(x => x.Quantity * x.Product.Price)
                })
                .OrderByDescending(x => x.TotalQuantity)
                .Take(10)
                .ToListAsync();
        }

        public async Task<IEnumerable<GetTopCustomerDto>> GetTopCustomers()
        {
            return await _context.Orders
                .GroupBy(o => new { o.UserId, o.User.Name })
                .Select(g => new GetTopCustomerDto
                {
                    UserId = g.Key.UserId,
                    UserName = g.Key.Name,
                    TotalSpent = g.Sum(x => x.TotalPrice)
                })
                .OrderByDescending(x => x.TotalSpent)
                .Take(10)
                .ToListAsync();
        }
    }
}
