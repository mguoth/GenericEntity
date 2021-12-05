using GenericEntity.Abstractions;
using SchemaModel = Schema.Model;
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

        public SchemaModel.SchemaDefinition GetSchema(string name)
        {
            string schemaFileName = Path.Combine(this.schemaDirectory, $"{name}.json");

            SchemaModel.SchemaDefinition schemaDefinition = JsonConvert.DeserializeObject<SchemaModel.SchemaDefinition>(File.ReadAllText(schemaFileName));
            return schemaDefinition;
        }
    }
}
