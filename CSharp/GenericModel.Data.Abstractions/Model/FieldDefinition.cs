using System;
using System.Collections.Generic;
using System.Text;

namespace GenericModel.Entity.Abstractions
{
    public class FieldDefinition
    {
        public string RawSchema { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public Type ValueType { get; set; }
        public object DefaultValue { get; set; }
        public bool Nullable { get; set; }
    }
}
