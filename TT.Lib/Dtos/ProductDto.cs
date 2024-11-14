using System.Collections.Generic;
using TT.Lib.Dtos;

namespace TT.Api.Models
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } 
        public string BrandName { get; set; }
        public int ProductPropertyId { get; set; }
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }

        // Properties used for Export Controller 
        public string Key { get; set; }
        public int BrandId { get; set; }
        public List<ProductPropertyDto> Properties { get; set; }
    }
}
