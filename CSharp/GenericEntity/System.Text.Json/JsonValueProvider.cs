using GenericEntity.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace GenericEntity.System.Text.Json
{
    public class JsonValueProvider : IJsonValueProvider
    {
        private readonly JsonElement? jsonElement;
        private readonly JsonDocument jsonDocument;

        public JsonValueProvider(JsonElement jsonElement)
        {
            this.jsonElement = jsonElement;
        }

        public JsonValueProvider(JsonDocument jsonDocument)
        {
            this.jsonDocument = jsonDocument;
        }

        public string GetString()
        {
            if (!jsonElement.HasValue)
            {
                throw new InvalidOperationException();
            }

            return jsonElement.Value.GetString();
        }

        public int GetInteger()
        {
            if (!jsonElement.HasValue)
            {
                throw new InvalidOperationException();
            }

            return jsonElement.Value.GetInt32();
        }
    }
}
