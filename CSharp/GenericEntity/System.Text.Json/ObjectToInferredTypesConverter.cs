using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GenericEntity
{
    /// <summary>
    /// Newtsonsoft like inferred type converter
    /// https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-converters-how-to?pivots=dotnet-core-3-1#deserialize-inferred-types-to-object-properties
    /// </summary>
    /// <seealso cref="System.Text.Json.Serialization.JsonConverter{System.Object}" />
    public class ObjectToInferredTypesConverter : JsonConverter<object>
    {
        public override object Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.True: return true;
                case JsonTokenType.False: return false;
                case JsonTokenType.Number:
                    if (reader.TryGetInt64(out long longValue))
                    {
                        return longValue;
                    }
                    return reader.GetDouble();
                case JsonTokenType.String:
                    if (reader.TryGetDateTime(out DateTime datetime))
                    {
                        return datetime;
                    }
                    return reader.GetString();
                default:
                    return JsonDocument.ParseValue(ref reader).RootElement.Clone();
            }
        }

        public override void Write(
            Utf8JsonWriter writer,
            object objectToWrite,
            JsonSerializerOptions options) =>
            JsonSerializer.Serialize(writer, objectToWrite, objectToWrite.GetType(), options);
    }
}
