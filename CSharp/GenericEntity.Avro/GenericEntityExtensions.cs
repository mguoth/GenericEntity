using GenericEntity.Abstractions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace GenericEntity.Abstractions
{
    public static class GenericEntityExtensions
    {
        /// <summary>
        /// Adds the Avro extension.
        /// </summary>
        /// <param name="extensions">The extensions.</param>
        public static void AddAvro(this IGenericEntityExtensions extensions)
        {
            extensions.AddExtension(Assembly.GetExecutingAssembly());
        }
    }
}
