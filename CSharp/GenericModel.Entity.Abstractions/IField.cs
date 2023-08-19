using System;

namespace GenericModel.Entity.Abstractions
{
    /// <summary>
    /// Field interface
    /// </summary>
    public interface IField
    {
        /// <summary>
        /// Gets the raw definition.
        /// </summary>
        string RawSchema { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the value type.
        /// </summary>
        Type ValueType { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IField"/> is nullable.
        /// </summary>
        bool Nullable { get; }

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

        /// <summary>
        /// Tries get the value and converts it to <see cref="TTarget"/>.
        /// </summary>
        /// <typeparam name="TTarget">The target type</typeparam>
        bool TryGetValue<TTarget>(out TTarget value);

        /// <summary>
        /// Tries set the value with prior conversion into <see cref="ValueType"/>.
        /// </summary>
        /// <typeparam name="TSource">The source type</typeparam>
        /// <param name="value">The value.</param>
        bool TrySetValue<TSource>(TSource value);
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
