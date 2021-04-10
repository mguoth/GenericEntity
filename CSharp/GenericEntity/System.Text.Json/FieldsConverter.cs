using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GenericEntity
{
    public class FieldsConverter : JsonConverter<IDictionary<string, object>>
    {
        private readonly static JsonConverter<object> valueConverter = new ObjectToInferredTypesConverter();

        public override IDictionary<string, object> Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var dictionary = new Dictionary<string, object>();

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
                if (valueConverter != null)
                {
                    reader.Read();
                    value = valueConverter.Read(ref reader, typeof(object), options);
                }
                else
                {
                    value = JsonSerializer.Deserialize<object>(ref reader, options);
                }

                // Add to dictionary.
                dictionary.Add(propertyName, value);
            }

            throw new JsonException();
        }

        public override void Write(
            Utf8JsonWriter writer,
            IDictionary<string, object> objectToWrite,
            JsonSerializerOptions options) =>
            JsonSerializer.Serialize(writer, objectToWrite, objectToWrite.GetType(), options);
    }
}
