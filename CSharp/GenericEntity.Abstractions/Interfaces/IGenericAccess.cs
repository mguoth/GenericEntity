using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity.Abstractions
{
    public interface IGenericAccess<T>
    {
        /// <summary>
        /// Gets a value indicating whether Value getter is safe.
        /// </summary>
        bool IsGetSafe { get; }

        /// <summary>
        /// Gets a value indicating whether Value setter is safe.
        /// </summary>
        bool IsSetSafe { get; }

        /// <summary>
        /// Gets or sets the value or throws Exception.
        /// </summary>
        T Value { get; set;  }
    }
}
