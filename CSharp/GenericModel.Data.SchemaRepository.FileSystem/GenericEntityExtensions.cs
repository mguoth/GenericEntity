using GenericModel.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace GenericModel.Data.Abstractions
{
    public static class GenericEntityExtensions
    {
        /// <summary>
        /// Adds the file system schema repository.
        /// </summary>
        /// <param name="extensions">The extensions.</param>
        public static void AddFileSystemSchemaRepository(this IGenericEntityExtensions extensions)
        {
            extensions.AddExtension(Assembly.GetExecutingAssembly());
        }
    }
}
