using System.Collections.Generic;
using System.Threading.Tasks;
using TT.Api.Models;

namespace TT.Lib.Services.Contracts
{
    public interface IDatabaseService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync();
    }
}
