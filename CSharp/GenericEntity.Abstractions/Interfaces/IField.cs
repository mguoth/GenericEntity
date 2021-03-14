using System;

namespace GenericEntity.Abstractions
{
    /// <summary>
    /// Field interface
    /// </summary>
    public interface IField
    {
        /// <summary>
        /// Gets field definition
        /// </summary>
        IFieldDefinition Definition { get; }

        /// <summary>
        /// Gets field value type
        /// </summary>
        /// <<inheritdoc />
        Type DataType { get; }
    }
}
