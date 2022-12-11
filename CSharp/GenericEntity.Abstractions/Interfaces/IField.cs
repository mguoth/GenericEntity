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
        FieldDefinition Definition { get; }

        /// <summary>
        /// Gets field value type.
        /// </summary>
        /// <<inheritdoc />
        Type ValueType { get; }

        /// <summary>
        /// Gets the value and converts it to <see cref="TTarget"/>.
        /// </summary>
        /// <typeparam name="TTarget">The target type</typeparam>
        /// <exception cref="InvalidOperationException">In case the type conversion has failed</exception>
        TTarget GetValue<TTarget>();

        /// <summary>
        /// Sets the value with prior conversion into <see cref="ValueType"/>.
        /// </summary>
        /// <typeparam name="TSource">The source type</typeparam>
        /// <param name="value">The value.</param>
        /// <exception cref="InvalidOperationException">In case the type conversion has failed</exception>
        void SetValue<TSource>(TSource value);
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
