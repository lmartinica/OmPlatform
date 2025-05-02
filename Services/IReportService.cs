using OmPlatform.DTOs.Order;
using OmPlatform.DTOs.Reports;

namespace OmPlatform.Services
{
    public interface IReportService
    {
        Task<IEnumerable<GetSaleMonthDto>> GetSales();
        Task<IEnumerable<GetTopProductDto>> GetTopProducts();
        Task<IEnumerable<GetTopCustomerDto>> GetTopCustomers();
    }
}