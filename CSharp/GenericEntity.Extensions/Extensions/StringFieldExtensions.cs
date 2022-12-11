using GenericEntity.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity.Extensions
{
    public static class StringFieldExtensions
    {
        /// <summary>
        /// Gets the value and converts it to <see cref="String"/>.
        /// </summary>
        public static string GetString(this IField field)
        {
            return field.GetValue<string>();
        }
    }
}
