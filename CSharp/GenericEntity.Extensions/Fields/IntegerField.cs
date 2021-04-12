using GenericEntity.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity.Extensions
{
    [FieldType("integer")]
    public class IntegerField : Field<int>
    {
        public IntegerField(IFieldDefinition fieldDefinition): base(fieldDefinition)
        {
        }
    }
}
