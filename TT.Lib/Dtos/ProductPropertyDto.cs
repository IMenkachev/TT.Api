using TT.Api.Models;

namespace TT.Lib.Dtos
{
    public class ProductPropertyDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int PropertyId { get; set; }
        public string Value { get; set; }
        public PropertyDto Property { get; set; }
    }
}
