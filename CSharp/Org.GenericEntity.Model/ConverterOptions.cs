using System;
using System.Collections.Generic;
using System.Text;

namespace Org.GenericEntity.Model
{
    /// <summary>
    /// Converter options
    /// </summary>
    public class ConverterOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether field name uses case insensitive comparison during the conversion.
        /// </summary>
        public bool FieldNameCaseInsensitive { get; set; } = false;
    }
}
