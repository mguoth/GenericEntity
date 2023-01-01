using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GenericEntity
{
    internal class GenericEntityDto
    {
        private IDictionary<string, object> data = new Dictionary<string, object>();

        public string SchemaUri { get; set; }
        public string SchemaFormat { get; set; }

        [JsonConverter(typeof(GenericEntityDtoFieldsConverter))]
        public IDictionary<string, object> Data { get => data; set => data = value; }
    }
}
