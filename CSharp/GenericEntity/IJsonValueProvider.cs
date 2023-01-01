using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity
{
    internal interface IFieldValueProvider
    {
        string GetString();

        int GetInt32();
    }
}
