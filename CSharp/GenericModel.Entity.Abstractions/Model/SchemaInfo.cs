using System;
using System.Collections.Generic;
using System.Text;

namespace GenericModel.Entity.Abstractions
{
    public class SchemaInfo
    {
        public string Id { get; set; }
        public string Format { get; set; }
        public string RawSchema { get; set; }
        public Uri Uri { get; set; }
    }
}
