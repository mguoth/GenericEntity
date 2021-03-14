using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity.Abstractions
{
    public class FieldTypeAttribute : Attribute
    {
        public FieldTypeAttribute(string fieldDefinitionType)
        {
            this.FieldDefinitionType = fieldDefinitionType;
        }

        public string FieldDefinitionType { get; }
    }
}
