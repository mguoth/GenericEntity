using GenericEntity.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity.Extensions
{
    [FieldType("string")]
    public class StringField : Field<string>
    {
        public StringField(IFieldDefinition fieldDefinition) : base(fieldDefinition)
        {
        }
    }
}
