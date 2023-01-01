using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity.Abstractions
{
    public interface ISchemaRepository
    {
        /// <summary>
        /// Gets the schema information.
        /// </summary>
        /// <param name="id">The schema identifier.</param>
        /// <returns></returns>
        SchemaInfo GetSchema(string id);
    }
}
