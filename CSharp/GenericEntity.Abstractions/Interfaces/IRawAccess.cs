using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity.Abstractions
{
    public interface IRawAccess
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        object Value { get; set; }
    }
}
