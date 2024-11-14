using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TT.Lib.Services.Contracts;

namespace TT.Api.Controllers
{
    [Route("data")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly ILogger<DataController> _logger;

        public DataController(IDatabaseService databaseService, ILogger<DataController> logger)
        {
            _databaseService = databaseService;
            _logger = logger;
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _databaseService.GetAllProductsAsync();
                return Ok(products);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error while fetching products.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("properties")]
        public async Task<IActionResult> GetAllProperties()
        {
            try
            {
                var properties = await _databaseService.GetAllPropertiesAsync();
                return Ok(properties);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error while fetching properties.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
