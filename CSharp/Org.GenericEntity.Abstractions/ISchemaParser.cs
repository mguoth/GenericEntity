using System;
using System.Collections.Generic;
using System.Text;

namespace Org.GenericEntity.Abstractions
{
    public interface ISchemaParser
    {
        GenericSchema Parse(string rawSchema);
    }
}
