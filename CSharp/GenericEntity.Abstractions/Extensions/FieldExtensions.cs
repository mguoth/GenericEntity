using GenericEntity.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity
{
    public static class FieldExtensions
    {
        public static IRawAccess AsRaw(this IField field)
        {
            return (IRawAccess) field;
        }
    }
}
