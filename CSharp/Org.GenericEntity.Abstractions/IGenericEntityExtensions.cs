using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Org.GenericEntity.Abstractions
{
    public interface IGenericEntityExtensions
    {
        /// <summary>
        /// Adds the extension assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        void AddExtension(Assembly assembly);
    }
}
