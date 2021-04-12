using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity.Abstractions
{
    /// <summary>
    /// Field value getter interface
    /// </summary>
    /// <typeparam name="T">The field value type</typeparam>
    public interface IGetter<T>
    {
        /// <summary>
        /// Gets the value
        /// </summary>
        T Value { get; }
    }
}
