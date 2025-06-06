using OmPlatform.DTOs.Reports;
using OmPlatform.Models;

namespace OmPlatform.Interfaces
{
    public interface IReportRepository
    {
        Task<IEnumerable<GetSaleMonthDto>> GetSales();
        Task<IEnumerable<GetTopProductDto>> GetTopProducts();
        Task<IEnumerable<GetTopCustomerDto>> GetTopCustomers();
    }
}
