using Microsoft.Extensions.Caching.Memory;
using OmPlatform.Core;
using OmPlatform.DTOs.Order;
using OmPlatform.DTOs.Reports;
using OmPlatform.Models;
using OmPlatform.Repositories;

namespace OmPlatform.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repository;

        public ReportService(IReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<GetSaleMonthDto>> GetSales()
        {
            return await _repository.GetSales();
        }

        public async Task<IEnumerable<GetTopProductDto>> GetTopProducts()
        {
            return await _repository.GetTopProducts();
        }

        public async Task<IEnumerable<GetTopCustomerDto>> GetTopCustomers()
        {
            return await _repository.GetTopCustomers();
        }
    }
}
