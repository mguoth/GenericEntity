using System;
using System.Collections.Generic;
using System.Text;

namespace Org.GenericEntity.Model
{
    internal interface IFieldValueProvider
    {
        string GetString();

        int GetInt32();
    }
}
