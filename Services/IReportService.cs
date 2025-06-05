using OmPlatform.Core;
using OmPlatform.DTOs.Order;
using OmPlatform.DTOs.Reports;

namespace OmPlatform.Services
{
    public interface IReportService
    {
        Task<Result<IEnumerable<GetSaleMonthDto>>> GetSales();
        Task<Result<IEnumerable<GetTopProductDto>>> GetTopProducts();
        Task<Result<IEnumerable<GetTopCustomerDto>>> GetTopCustomers();
    }
}