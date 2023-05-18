using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GenericModel.Data
{
    internal class GenericEntityDto
    {
        private IDictionary<string, object> data = new Dictionary<string, object>();

        public Uri SchemaUri { get; set; }
        public string SchemaFormat { get; set; }

        [JsonConverter(typeof(GenericEntityDtoFieldsConverter))]
        public IDictionary<string, object> Data { get => data; set => data = value; }
    }
}
