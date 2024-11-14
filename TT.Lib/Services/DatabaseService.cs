using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TT.Api.Models;
using TT.Lib.Services.Contracts;

namespace TT.Lib.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IConfiguration _configuration;

        public DatabaseService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = new List<ProductDto>();
            string connectionString = _configuration.GetConnectionString("TTDbContext");

            await ServiceHelper.ExecuteReaderAsync(
                connectionString,
                "GetAllProducts",
                CommandType.StoredProcedure,
                async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        products.Add(new ProductDto
                        {
                            ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                            ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                            BrandName = reader.GetString(reader.GetOrdinal("BrandName")),
                            ProductPropertyId = reader.IsDBNull(reader.GetOrdinal("ProductPropertyId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ProductPropertyId")),
                            PropertyName = reader.IsDBNull(reader.GetOrdinal("PropertyName")) ? null : reader.GetString(reader.GetOrdinal("PropertyName")),
                            PropertyValue = reader.IsDBNull(reader.GetOrdinal("PropertyValue")) ? null : reader.GetString(reader.GetOrdinal("PropertyValue"))
                        });
                    }
                }
            );

            return products;
        }

        public async Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync()
        {
            var properties = new List<PropertyDto>();
            string connectionString = _configuration.GetConnectionString("TTDbContext");

            await ServiceHelper.ExecuteReaderAsync(
                connectionString,
                "SELECT * FROM vw_AllProperties",
                CommandType.Text,
                async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        properties.Add(new PropertyDto
                        {
                            PropertyId = reader.GetInt32(reader.GetOrdinal("PropertyId")),
                            PropertyName = reader.GetString(reader.GetOrdinal("PropertyName")),
                            ParentId = (int)(reader.IsDBNull(reader.GetOrdinal("ParentId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ParentId"))),
                            PropertyType = reader.IsDBNull(reader.GetOrdinal("PropertyType")) ? null : reader.GetString(reader.GetOrdinal("PropertyType"))
                        });
                    }
                }
            );

            return properties;
        }
    }
}
