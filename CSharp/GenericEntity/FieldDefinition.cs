using System;
using System.Collections.Generic;
using System.Text;
using SchemaModel = Schema.Model;

namespace GenericEntity.Abstractions
{
    public class FieldDefinition : IFieldDefinition
    {
        private readonly SchemaModel.FieldDefinition data;

        public FieldDefinition(ISchema schema, Type fieldType, SchemaModel.FieldDefinition data)
        {
            this.Schema = schema;
            this.FieldType = fieldType;
            this.data = data;
        }

        public ISchema Schema { get; }
        public string Name => this.data.Name;
        public string Type => this.data.Type;
        public string DisplayName => this.data.DisplayName;
        public string Description => this.data.Description;
        public Type FieldType { get; }
    }
}
