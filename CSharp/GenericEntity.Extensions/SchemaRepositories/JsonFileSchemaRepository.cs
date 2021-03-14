using GenericEntity.Abstractions;
using GenericEntity.Model;
using Newtonsoft.Json;
using System.IO;

namespace GenericEntity.Extensions
{
    public class JsonFileSchemaRepository : ISchemaRepository
    {
        private string schemaDirectory;

        public JsonFileSchemaRepository(string schemaDirectory)
        {
            this.schemaDirectory = schemaDirectory;
        }

        public SchemaDefinition GetSchema(string name)
        {
            string schemaFileName = Path.Combine(this.schemaDirectory, $"{name}.json");

            SchemaDefinition schemaDefinition = JsonConvert.DeserializeObject<SchemaDefinition>(File.ReadAllText(schemaFileName));
            return schemaDefinition;
        }
    }
}
