using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity.Abstractions
{
    public class SchemaCompilerData
    {
        public SchemaCompilerData(IDictionary<string, Type> fieldTypes)
        {
            this.FieldTypes = fieldTypes;
        }

        public IDictionary<string, Type> FieldTypes { get; }
    }
}
