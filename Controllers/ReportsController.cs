using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmPlatform.Core;
using OmPlatform.DTOs.Product;
using OmPlatform.Models;
using OmPlatform.Services;

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
            var entitites = await _reportService.GetSales();
            return Ok(entitites);
        }

        [HttpGet("topProducts")]
        public async Task<ActionResult<IEnumerable<GetProductDto>>> GetTopProducts()
        {
            var entitites = await _reportService.GetTopProducts();
            return Ok(entitites);
        }

        [HttpGet("topCustomers")]
        public async Task<ActionResult<IEnumerable<GetProductDto>>> GetTopCustomers()
        {
            var entitites = await _reportService.GetTopCustomers();
            return Ok(entitites);
        }
    }
}
