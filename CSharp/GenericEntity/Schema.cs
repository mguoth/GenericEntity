using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GenericEntity.Abstractions;

namespace GenericEntity
{
    public class Schema : ISchema
    {      
        private readonly global::Schema.Model.SchemaDefinition data;

        public Schema(global::Schema.Model.SchemaDefinition data, SchemaCompilerData compilerData)
        {
            this.data = data;
            this.Fields = this.data.Fields.Select(x => new FieldDefinition(this, compilerData.FieldTypes[x.Type], x)).ToArray();
        }

        public string Namespace => this.data.Namespace;
        public string EntityType => this.data.EntityType;
        public IList<IFieldDefinition> Fields { get; }
    }
}
