using GenericEntity.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity.Extensions
{
    public static class IntegerFieldExtensions
    {
        /// <summary>
        /// Gets the value and converts it to integer.
        /// </summary>
        /// <exception cref="InvalidOperationException">In case the type conversion has failed</exception>
        public static int GetInteger(this IField field)
        {
            return field.GetValue<int>();
        }
    }
}
