using GenericEntity.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity
{
    public class StringField : Field<string>
    {
        public StringField(FieldDefinition fieldDefinition) : base(fieldDefinition)
        {
        }
    }
}
