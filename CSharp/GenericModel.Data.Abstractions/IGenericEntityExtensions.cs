using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GenericModel.Data.Abstractions
{
    public interface IGenericEntityExtensions
    {
        /// <summary>
        /// Adds the extension assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        void AddExtension(Assembly assembly);

        /// <summary>
        /// Gets the schema repository.
        /// </summary>
        /// <param name="name">The name.</param>
        ISchemaRepository GetSchemaRepository(string name);

        /// <summary>
        /// Gets the schema parser.
        /// </summary>
        /// <param name="format">The format.</param>
        ISchemaParser GetSchemaParser(string format);
    }
}
