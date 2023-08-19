using System;
using System.Collections.Generic;
using System.Text;

namespace GenericModel.Entity.Abstractions
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
