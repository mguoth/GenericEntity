using GenericEntity.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity
{
    public static class FieldExtensions
    {
        public static IGenericAccess<string> AsString(this IField field)
        {
            return field.As<string>();
        }

        public static IGenericAccess<int> AsInt32(this IField field)
        {
            return field.As<int>();
        }
    }
}
