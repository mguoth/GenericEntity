using GenericEntity.Abstractions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace GenericEntity.Extensions
{
    [SchemaRepository("memory")]
    public class InMemorySchemaRepository : ISchemaRepository, IEnumerable<(string Id, string Format, string Payload)>
    {
        private static readonly IDictionary<string, (string Id, string Format, string Payload)> schemas = new Dictionary<string, (string Id, string Format, string Payload)>();

        public IEnumerator<(string Id, string Format, string Payload)> GetEnumerator()
        {
            return schemas.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(string schemaId, string schemaFormat, string schemaPayload)
        {
            schemas.Add(schemaId, (schemaId, schemaFormat, schemaPayload));
        }

        /// <inheritdoc/>
        public SchemaInfo GetSchema(string id)
        {
            var schema = schemas[id];
            return new SchemaInfo() { Id = id, Format = schema.Format, Payload = schema.Payload, Uri = new UriBuilder("memory", "localhost") { Path = $"{id}" }.Uri.ToString() };
        }
    }
}
