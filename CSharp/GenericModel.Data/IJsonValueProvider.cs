using System;
using System.Collections.Generic;
using System.Text;

namespace GenericModel.Data
{
    internal interface IFieldValueProvider
    {
        string GetString();

        int GetInt32();
    }
}
