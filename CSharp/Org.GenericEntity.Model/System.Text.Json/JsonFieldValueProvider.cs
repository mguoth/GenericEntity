using Org.GenericEntity.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Org.GenericEntity.Model.System.Text.Json
{
    internal class JsonFieldValueProvider : IFieldValueProvider
    {
        private readonly JsonElement? jsonElement;

        public JsonFieldValueProvider(JsonElement jsonElement)
        {
            this.jsonElement = jsonElement;
        }

        public string GetString()
        {
            if (!jsonElement.HasValue)
            {
                throw new InvalidOperationException();
            }

            return jsonElement.Value.GetString();
        }

        public int GetInt32()
        {
            if (!jsonElement.HasValue)
            {
                throw new InvalidOperationException();
            }

            return jsonElement.Value.GetInt32();
        }
    }
}
