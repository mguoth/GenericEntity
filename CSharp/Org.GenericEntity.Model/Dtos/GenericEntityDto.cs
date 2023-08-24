using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text.Json.Serialization;

namespace Org.GenericEntity.Model
{
    [JsonConverter(typeof(GenericEntityDtoConverter))]
    internal class GenericEntityDto : Dictionary<string, object>
    {
        public IEnumerable<KeyValuePair<string, object>> Fields => this.Where(x => x.Key != "$genericEntity");

        [JsonIgnore]
        public GenericEntityInfo GenericEntityInfo
        {
            get
            {
                return (GenericEntityInfo) this["$genericEntity"];
            }
            set
            {
                this["$genericEntity"] = value;
            }
        }
    }
}
