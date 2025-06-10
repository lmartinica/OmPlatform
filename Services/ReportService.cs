using OmPlatform.Core;
using OmPlatform.DTOs.Reports;
using OmPlatform.Interfaces;

namespace OmPlatform.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repository;

        public ReportService(IReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<IEnumerable<GetSaleMonthDto>>> GetSales()
        {
            return Result<IEnumerable<GetSaleMonthDto>>.Success(await _repository.GetSales());
        }

        public async Task<Result<IEnumerable<GetTopProductDto>>> GetTopProducts()
        {
            return Result<IEnumerable<GetTopProductDto>>.Success(await _repository.GetTopProducts());
        }

        public async Task<Result<IEnumerable<GetTopCustomerDto>>> GetTopCustomers()
        {
            return Result<IEnumerable<GetTopCustomerDto>>.Success(await _repository.GetTopCustomers());
        }
    }
}
