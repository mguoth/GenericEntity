using GenericEntity.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity.Abstractions
{
    public interface ISchemaRepository
    {
        /// <summary>
        /// Gets the schema.
        /// </summary>
        /// <param name="name">The schema name.</param>
        /// <returns></returns>
        SchemaDefinition GetSchema(string name);
    }
}
