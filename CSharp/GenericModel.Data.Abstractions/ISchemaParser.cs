using System;
using System.Collections.Generic;
using System.Text;

namespace GenericModel.Entity.Abstractions
{
    public interface ISchemaParser
    {
        GenericSchema Parse(string rawSchema);
    }
}
