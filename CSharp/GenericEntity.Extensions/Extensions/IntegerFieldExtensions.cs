using GenericEntity.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity.Extensions
{
    public static class IntegerFieldExtensions
    {
        /// <summary>
        /// Gets the value and converts it to <see cref="Int32"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">In case the type conversion has failed</exception>
        public static int GetInt32(this IField field)
        {
            return field.GetValue<int>();
        }
    }
}
