using GenericEntity.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity
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
