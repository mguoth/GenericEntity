using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity.Abstractions
{
    /// <summary>
    /// Field value setter interface
    /// </summary>
    /// <typeparam name="T">The field value type</typeparam>
    public interface ISetter<T> : IGetterSetterSupported
    {
        /// <summary>
        /// Sets the value in case it is supported otherwise throws NotSupportedException
        /// <exception cref="NotSupportedException"></exception>
        /// </summary>
        T Value { set; }
    }
}
