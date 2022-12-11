using System;
using System.Collections.Generic;
using System.Text;
using SchemaModel = Schema.Model;

namespace GenericEntity.Abstractions
{
    public class FieldDefinition
    {
        private readonly SchemaModel.FieldDefinition data;

        public FieldDefinition(Schema schema, SchemaModel.FieldDefinition data)
        {
            this.Schema = schema;
            this.data = data;
        }

        public Schema Schema { get; }
        public string Name => this.data.Name;
        public string Type => this.data.Type;
        public string DisplayName => this.data.DisplayName;
        public string Description => this.data.Description;
    }
}
