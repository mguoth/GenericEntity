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
        /// Adds the standard extensions.
        /// </summary>
        /// <param name="extensions">The extensions.</param>
        public static void AddStandard(this IGenericEntityExtensions extensions)
        {
            extensions.AddExtension(Assembly.GetExecutingAssembly());
        }
    }
}
