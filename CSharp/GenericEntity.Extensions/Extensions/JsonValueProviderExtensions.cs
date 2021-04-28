using GenericEntity.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity.Extensions
{
    public static class JsonValueProviderExtensions
    {
        public static bool GetBool(this IJsonValueProvider jsonValueProvider)
        {
            return true;
        }
    }
}
