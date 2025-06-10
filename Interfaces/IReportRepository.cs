using OmPlatform.DTOs.Reports;

namespace OmPlatform.Interfaces
{
    public interface IReportRepository
    {
        Task<IEnumerable<GetSaleMonthDto>> GetSales();
        Task<IEnumerable<GetTopProductDto>> GetTopProducts();
        Task<IEnumerable<GetTopCustomerDto>> GetTopCustomers();
    }
}
