using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity.Abstractions
{
    public interface IJsonValueProvider
    {
        string GetString();

        int GetInteger();
    }
}
