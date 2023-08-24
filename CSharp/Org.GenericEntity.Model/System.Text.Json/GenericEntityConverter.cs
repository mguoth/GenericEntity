using Org.GenericEntity.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Org.GenericEntity.Model
{
    internal class GenericEntityConverter : JsonConverter<GenericEntity>
    {
        public override GenericEntity Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            GenericEntityDto dto = JsonSerializer.Deserialize<GenericEntityDto>(ref reader, options);
            SchemaInfo schemaInfo = RetrieveSchemaInfo(dto);

            //Override data from DTO
            schemaInfo.Format = dto.GenericEntityInfo.SchemaFormat;
            schemaInfo.Uri = dto.GenericEntityInfo.SchemaUri;

            ISchemaParser schemaParser = ((GenericEntityExtensions) GenericEntity.Extensions).GetSchemaParser(schemaInfo.Format);
            GenericEntity genericEntity = new GenericEntity(dto, schemaInfo, schemaParser);

            return genericEntity;
        }

        public override void Write(
            Utf8JsonWriter writer,
            GenericEntity objectToWrite,
            JsonSerializerOptions options)
        {
            GenericEntityDto dto = new GenericEntityDto();
            dto.GenericEntityInfo = new GenericEntityInfo();
            dto.GenericEntityInfo.SchemaUri = objectToWrite.SchemaInfo.Uri;
            dto.GenericEntityInfo.SchemaFormat = objectToWrite.SchemaInfo.Format;

            foreach (IField field in objectToWrite.Fields)
            {
                dto.Add(field.Name, field.GetValue<object>());
            }

            JsonSerializer.Serialize(writer, dto, dto.GetType(), new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        private static SchemaInfo RetrieveSchemaInfo(GenericEntityDto dto)
        {
            ISchemaRepository schemaRepository = ((GenericEntityExtensions) GenericEntity.Extensions).GetSchemaRepository(dto.GenericEntityInfo.SchemaUri.Scheme);

            SchemaInfo schemaInfo = schemaRepository.GetSchema(dto.GenericEntityInfo.SchemaUri);
            return schemaInfo;
        }
    }
}
