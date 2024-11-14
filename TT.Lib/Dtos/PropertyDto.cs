using System.Collections.Generic;

namespace TT.Api.Models
{
    public class PropertyDto
    {
        public int PropertyId { get; set; }
        public string PropertyName { get; set; }
        public string PropertyType { get; set; }

        // Properties used for Export Controller 
        public int ParentId { get; set; }
        public string Value { get; set; }
        public Dictionary<string, object> SubProperties { get; set; }
    }
}
