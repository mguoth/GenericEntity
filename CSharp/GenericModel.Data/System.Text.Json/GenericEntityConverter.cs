using GenericModel.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GenericModel.Data
{
    public class GenericEntityConverter : JsonConverter<GenericEntity>
    {
        public override GenericEntity Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            GenericEntityDto dto = JsonSerializer.Deserialize<GenericEntityDto>(ref reader, options);
            SchemaInfo schemaInfo = RetrieveSchemaInfo(dto);

            //Override data from DTO
            schemaInfo.Format = dto.SchemaFormat;
            schemaInfo.Uri = dto.SchemaUri;

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
            dto.SchemaUri = objectToWrite.SchemaInfo.Uri;
            dto.SchemaFormat = objectToWrite.SchemaInfo.Format;

            foreach (IField field in objectToWrite.Fields)
            {
                dto.Data.Add(field.Name, field.GetValue<object>());
            }

            JsonSerializer.Serialize(writer, dto, dto.GetType(), options);
        }

        private static SchemaInfo RetrieveSchemaInfo(GenericEntityDto dto)
        {
            ISchemaRepository schemaRepository = ((GenericEntityExtensions) GenericEntity.Extensions).GetSchemaRepository(dto.SchemaUri.Scheme);

            SchemaInfo schemaInfo = schemaRepository.GetSchema(dto.SchemaUri);
            return schemaInfo;
        }
    }
}
