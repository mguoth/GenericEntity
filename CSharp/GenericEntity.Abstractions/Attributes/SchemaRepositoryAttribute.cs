using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity.Abstractions
{
    public class SchemaRepositoryAttribute: Attribute
    {
        public SchemaRepositoryAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}
