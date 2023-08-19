using Org.GenericEntity.Abstractions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Org.GenericEntity.Abstractions
{
    public static class GenericEntityExtensions
    {
        /// <summary>
        /// Adds the Avro schema.
        /// </summary>
        /// <param name="extensions">The extensions.</param>
        public static void AddAvroSchema(this IGenericEntityExtensions extensions)
        {
            extensions.AddExtension(Assembly.GetExecutingAssembly());
        }
    }
}
