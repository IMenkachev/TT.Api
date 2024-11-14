using System;
using System.Collections.Generic;
using System.Text;

namespace TT.Lib.Entities
{
    public class Property : BaseName
    {
        public int ParentId { get; set; }

        // New Property to add
        public string Type { get; set; }  // Assuming it's a string type
    }
}
