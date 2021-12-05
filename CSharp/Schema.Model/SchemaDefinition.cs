using System;
using System.Collections.Generic;
using System.Text;

namespace Schema.Model
{
    public class SchemaDefinition
    {
        public string Namespace { get; set; }
        public string EntityType { get; set; }
        public IList<FieldDefinition> Fields { get; set; }
    }
}
