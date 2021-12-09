using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GenericEntity.Abstractions;
using SchemaModel = Schema.Model;


namespace GenericEntity
{
    public class Schema : ISchema
    {      
        private readonly SchemaModel.SchemaDefinition data;

        public Schema(SchemaModel.SchemaDefinition data, SchemaCompilerData compilerData)
        {
            this.data = data;
            this.Fields = this.data.Fields.Select(x => new FieldDefinition(this, compilerData.FieldTypes[x.Type], x)).ToArray();
        }

        public string Namespace => this.data.Namespace;
        public string Name => this.data.Name;
        public IList<IFieldDefinition> Fields { get; }
    }
}
