using System;

namespace GenericEntity.Abstractions
{
    /// <summary>
    /// Field interface
    /// </summary>
    public interface IField
    {
        /// <summary>
        /// Gets field definition.
        /// </summary>
        IFieldDefinition Definition { get; }

        /// <summary>
        /// Gets field value type.
        /// </summary>
        /// <<inheritdoc />
        Type DataType { get; }

        /// <summary>
        /// Gets the value and converts it to <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The target type</typeparam>
        /// <exception cref="InvalidOperationException">In case the type conversion has failed</exception>
        T GetValue<T>();

        /// <summary>
        /// Sets the value with prior conversion into DataType.
        /// </summary>
        /// <typeparam name="T">The source type</typeparam>
        /// <param name="value">The value.</param>
        /// <exception cref="InvalidOperationException">In case the type conversion has failed</exception>
        void SetValue<T>(T value);
    }

    /// <summary>
    /// Generic field interface
    /// </summary>
    public interface IField<T> : IField
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        T Value { get; set; }
    }
}
