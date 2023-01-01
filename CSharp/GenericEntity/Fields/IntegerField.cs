using GenericEntity.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity
{
    public class IntegerField : Field<int>
    {
        public IntegerField(FieldDefinition fieldDefinition): base(fieldDefinition)
        {
        }
    }
}
