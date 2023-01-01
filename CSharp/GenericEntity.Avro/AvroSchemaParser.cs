using Avro;
using Avro.Generic;
using GenericEntity.Abstractions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace GenericEntity.Avro
{
    [SchemaParser("avro")]
    public class AvroSchemaParser : ISchemaParser
    {
        /// <inheritdoc />
        GenericSchema ISchemaParser.Parse(string rawSchema)
        {
            RecordSchema schema = Schema.Parse(rawSchema) as RecordSchema;

            using (JsonDocument jsonDocument = JsonDocument.Parse(rawSchema))
            {
                List<string> fieldSchemas = jsonDocument.RootElement
                    .GetProperty("fields")
                    .EnumerateArray()
                    .Select(x => x.ToString())
                    .ToList();

                GenericSchema genericSchema = new GenericSchema();
                genericSchema.RawSchema = rawSchema;
                genericSchema.Fields = schema.Fields.Select((x, index) => new FieldDefinition() { RawSchema = fieldSchemas[index], Name = x.Name, Description = x.Documentation, FieldType = GetFieldType(x), DisplayName = x.GetProperty("displayName").Trim('\"') }).ToArray();
                return genericSchema;
            }
        }

        private Type GetFieldType(global::Avro.Field field)
        {
            if (field.Schema.Name == "string")
            {
                return typeof(StringField);
            }

            if (field.Schema.Name == "int")
            {
                return typeof(IntegerField);
            }

            throw new NotSupportedException();
        }
    }
}
