using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TT.Lib;
using TT.Lib.Dtos;
using TT.Lib.Services;

[Route("export")]
[ApiController]
public class ExportController : ControllerBase
{
    private readonly TTDbContext _context;
    private readonly ProductPropertiesService _propertyService;
    private readonly ILogger<ExportController> _logger;

    public ExportController(TTDbContext context, ProductPropertiesService propertyService, ILogger<ExportController> logger)
    {
        _context = context;
        _propertyService = propertyService;
        _logger = logger;
    }

    [HttpGet("product")]
    public async Task<IActionResult> GetProductExport()
    {
        try
        {
            var products = await _context.Products
                .Include(p => p.Brand)
                .ToListAsync();

            var productDtos = new List<ProductExportDto>();

            foreach (var product in products)
            {
                var brandDto = new BrandDto
                {
                    Id = product.Brand.Id,
                    BrandName = product.Brand.Name
                };

                // Fetch the property hierarchy for the product
                var rootProperties = await _propertyService.GetProductPropertiesAsync(product.Id);

                var productDto = new ProductExportDto
                {
                    Id = product.Key,
                    Name = product.Name,
                    Brand = brandDto,
                    Properties = rootProperties
                };

                productDtos.Add(productDto);
            }

            return Ok(productDtos);
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Database update error occurred while exporting product data.");
            return StatusCode(500, "An error occurred while accessing the database.");
        }
        catch (InvalidOperationException invEx)
        {
            _logger.LogError(invEx, "An invalid operation occurred while processing product data.");
            return StatusCode(500, "An error occurred while processing the product data.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while exporting product data.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}
