using System.Collections.Generic;

namespace TT.Lib.Dtos
{
    public class ProductExportDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public BrandDto Brand { get; set; }
        public Dictionary<string, object> Properties { get; set; }
    }
}
