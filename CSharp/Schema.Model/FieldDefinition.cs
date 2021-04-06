using System;
using System.Collections.Generic;
using System.Text;

namespace Schema.Model
{
    public class FieldDefinition
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }
}
