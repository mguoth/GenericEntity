using GenericEntity.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity.Extensions
{
    [FieldType("integer")]
    public class IntegerField : Field<int>, IGetter<string>
    {
        public IntegerField(IFieldDefinition fieldDefinition): base(fieldDefinition)
        {
        }

        string IGetter<string>.Value => this.Value.ToString();
    }
}
