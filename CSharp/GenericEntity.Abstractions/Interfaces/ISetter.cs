using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity.Abstractions
{
    /// <summary>
    /// Field value setter interface
    /// </summary>
    /// <typeparam name="T">The field value type</typeparam>
    public interface ISetter<T>
    {
        /// <summary>
        /// Sets the value
        /// </summary>
        T Value { set; }
    }
}
