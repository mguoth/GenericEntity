using System;
using System.Collections.Generic;
using System.Text;

namespace GenericModel.Entity.Abstractions
{
    public interface ISchemaRepository
    {
        /// <summary>
        /// Gets the schema information.
        /// </summary>
        /// <param name="id">The schema identifier.</param>
        /// <returns></returns>
        SchemaInfo GetSchema(string id);

        /// <summary>
        /// Gets the schema information.
        /// </summary>
        /// <param name="uri">The schema URI.</param>
        /// <returns></returns>
        SchemaInfo GetSchema(Uri uri);
    }
}
