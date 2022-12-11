using GenericEntity.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity
{
    public class CompiledSchema
    {
        public CompiledSchema(Abstractions.Schema schema, SchemaCompilerData compilerData)
        {
            Schema = schema;
            CompilerData = compilerData;
        }

        public Abstractions.Schema Schema { get; }
        public SchemaCompilerData CompilerData { get; }
    }
}
