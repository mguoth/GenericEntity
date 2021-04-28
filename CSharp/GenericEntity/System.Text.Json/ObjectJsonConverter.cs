using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GenericEntity.System.Text.Json
{
    public class ObjectJsonConverter : JsonConverter<object>
    {
        public override object Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    return null;
                case JsonTokenType.True:
                    return true;
                case JsonTokenType.False:
                    return false;
                case JsonTokenType.StartObject:
                    return new JsonValueProvider(JsonDocument.ParseValue(ref reader));
                case JsonTokenType.StartArray:
                    {
                        IList<object> list = new List<object>();

                        while (reader.Read())
                        {
                            if (reader.TokenType == JsonTokenType.EndArray)
                            {
                                break;
                            }

                            list.Add(this.Read(ref reader, typeof(object), options));
                        }
                        return list;
                    }

                default:
                    return new JsonValueProvider(JsonDocument.ParseValue(ref reader).RootElement.Clone());
            }
        }

        public override void Write(
            Utf8JsonWriter writer,
            object objectToWrite,
            JsonSerializerOptions options) =>
            JsonSerializer.Serialize(writer, objectToWrite, objectToWrite.GetType(), options);
    }
}
