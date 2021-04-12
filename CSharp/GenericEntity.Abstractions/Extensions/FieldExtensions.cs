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

        public static IGenericAccess<T> As<T>(this IField field)
        {
            IGenericAccess<T> implicitAccessor = field as IGenericAccess<T>;
            if (implicitAccessor != null)
            {
                return implicitAccessor;
            }
            return new GenericAccess<T>(field);
        }
    }
}
