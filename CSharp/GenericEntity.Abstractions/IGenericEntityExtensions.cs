using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GenericEntity.Abstractions
{
    public interface IGenericEntityExtensions
    {
        void RegisterExtension(Assembly assembly);
    }
}
