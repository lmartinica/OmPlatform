using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmPlatform.Core;
using OmPlatform.DTOs.Product;
using OmPlatform.Interfaces;
using OmPlatform.Models;

namespace OmPlatform.Controllers
{
    [ApiController]
    [Authorize(Roles = Constants.Admin)]
    [Route("[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("sales")]
        public async Task<ActionResult<IEnumerable<GetProductDto>>> GetSales()
        {
            var result = await _reportService.GetSales();
            return Ok(result.Data);
        }

        [HttpGet("topProducts")]
        public async Task<ActionResult<IEnumerable<GetProductDto>>> GetTopProducts()
        {
            var result = await _reportService.GetTopProducts();
            return Ok(result.Data);
        }

        [HttpGet("topCustomers")]
        public async Task<ActionResult<IEnumerable<GetProductDto>>> GetTopCustomers()
        {
            var result = await _reportService.GetTopCustomers();
            return Ok(result.Data);
        }
    }
}
