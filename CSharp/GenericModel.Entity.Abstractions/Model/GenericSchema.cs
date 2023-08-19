using System;
using System.Collections.Generic;
using System.Text;

namespace GenericModel.Entity.Abstractions
{
    public class GenericSchema
    {
        public string RawSchema { get; set; }
        public IList<FieldDefinition> Fields { get; set; }
    }
}
