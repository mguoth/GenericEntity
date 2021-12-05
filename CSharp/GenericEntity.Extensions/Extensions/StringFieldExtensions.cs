using GenericEntity.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity.Extensions
{
    public static class StringFieldExtensions
    {
        /// <summary>
        /// Gets the value and converts it to string.
        /// </summary>
        public static string GetString(this IField field)
        {
            return field.Get<string>();
        }

        /// <summary>
        /// Sets the string value with prior conversion into DataType.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <exception cref="InvalidOperationException">In case the type conversion has failed</exception>
        public static void SetString(this IField field, string value)
        {
            field.Set(value);
        }
    }
}
