using OmPlatform.Core;
using OmPlatform.DTOs.Reports;

namespace OmPlatform.Interfaces
{
    public interface IReportService
    {
        Task<Result<IEnumerable<GetSaleMonthDto>>> GetSales();
        Task<Result<IEnumerable<GetTopProductDto>>> GetTopProducts();
        Task<Result<IEnumerable<GetTopCustomerDto>>> GetTopCustomers();
    }
}