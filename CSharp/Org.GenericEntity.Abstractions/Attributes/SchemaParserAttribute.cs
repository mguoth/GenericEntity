using System;
using System.Collections.Generic;
using System.Text;

namespace Org.GenericEntity.Abstractions
{
    public class SchemaParserAttribute: Attribute
    {
        public SchemaParserAttribute(string format)
        {
            this.Format = format;
        }

        public string Format { get; }
    }
}
