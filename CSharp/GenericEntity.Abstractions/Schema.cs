using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SchemaModel = Schema.Model;

namespace GenericEntity.Abstractions
{
    public class Schema
    {      
        private readonly SchemaModel.SchemaDefinition data;

        public Schema(SchemaModel.SchemaDefinition data)
        {
            this.data = data;
            this.Fields = this.data.Fields.Select(x => new FieldDefinition(this, x)).ToArray();
        }

        public string Namespace => this.data.Namespace;
        public string Name => this.data.Name;
        public IList<FieldDefinition> Fields { get; }
    }
}
