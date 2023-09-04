using Org.GenericEntity.Model.System.Text.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Org.GenericEntity.Model
{
    internal class GenericEntityDtoConverter : JsonConverter<GenericEntityDto>
    {
        private readonly static ObjectJsonConverter ValueConverter = new ObjectJsonConverter();

        public override GenericEntityDto Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var dictionary = new GenericEntityDto();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return dictionary;
                }

                // Get the key.
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }
                string propertyName = reader.GetString();

                // Get the value.
                object value;
                if (propertyName == "$genericEntity")
                {
                    reader.Read();
                    value = JsonSerializer.Deserialize<GenericEntityInfo>(ref reader, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    reader.Read();
                    value = ValueConverter.Read(ref reader, typeof(object), options);
                }

                // Add to dictionary.
                dictionary.Add(propertyName, value);
            }

            throw new JsonException();
        }

        public override void Write(
            Utf8JsonWriter writer,
            GenericEntityDto objectToWrite,
            JsonSerializerOptions options) =>
            JsonSerializer.Serialize(writer, objectToWrite, typeof(IDictionary<string, object>), options);
    }
}
