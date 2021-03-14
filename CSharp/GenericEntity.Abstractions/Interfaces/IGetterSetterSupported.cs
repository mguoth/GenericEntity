using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity.Abstractions
{
    public interface IGetterSetterSupported
    {
        /// <summary>
        /// Gets a value indicating whether this getter or setter is supported.
        /// </summary>
        bool IsSupported { get; }
    }
}
