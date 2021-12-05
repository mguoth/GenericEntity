using GenericEntity.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GenericEntity
{
    public class GenericEntityConverter : JsonConverter<GenericEntity>
    {
        public override GenericEntity Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            GenericEntityDto dto = JsonSerializer.Deserialize<GenericEntityDto>(ref reader, options);
            GenericEntity genericEntity = new GenericEntity(dto, GenericEntity.DefaultSchemaRepository);

            return genericEntity;
        }

        public override void Write(
            Utf8JsonWriter writer,
            GenericEntity objectToWrite,
            JsonSerializerOptions options)
        {
            GenericEntityDto dto = new GenericEntityDto();
            dto.Schema = objectToWrite.Schema.EntityType;

            foreach (IField field in objectToWrite.Fields)
            {
                dto.Fields.Add(field.Definition.Name, field.Get<object>());
            }

            JsonSerializer.Serialize(writer, dto, dto.GetType(), options);
        }
    }
}
