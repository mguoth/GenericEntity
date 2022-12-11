using System;

namespace GenericEntity.Abstractions
{
    /// <summary>
    /// Base field class
    /// </summary>
    public abstract class Field : IField
    {
        public Field(FieldDefinition definition)
        {
            this.Definition = definition;
        }

        /// <inheritdoc/>
        public FieldDefinition Definition { get; }

        /// <inheritdoc/>
        public Type ValueType
        {
            get
            {
                return this.GetValueTypeInternal();
            }
        }

        /// <inheritdoc/>
        public TTarget GetValue<TTarget>()
        {
            if (this is IField<TTarget> field)
            {
                return field.Value;
            }

            //Try convert
            try
            {
                return (TTarget) Convert.ChangeType(this.GetValueInternal(), typeof(TTarget));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($@"Can't convert the field value of ""{this.ValueType}"" type into the ""{typeof(TTarget)}"" return type", ex);
            }
        }

        /// <inheritdoc/>
        public void SetValue<TSource>(TSource value)
        {
            this.SetValueInternal(value);
        }

        protected abstract Type GetValueTypeInternal();
        protected abstract void SetValueInternal<TSource>(TSource Value);
        protected abstract object GetValueInternal();
    }

    /// <summary>
    /// Generic base field class
    /// </summary>
    /// <typeparam name="T">The field value type</typeparam>
    public abstract class Field<T> : Field, IField<T>
    {
        public Field(FieldDefinition definition) : base(definition)
        {
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public T Value { get; set; }

        protected sealed override Type GetValueTypeInternal()
        {
            return typeof(T);
        }

        protected sealed override object GetValueInternal()
        {
            return this.Value;
        }

        protected sealed override void SetValueInternal<TRequested>(TRequested value)
        {
            if (value is T castedValue)
            {
                this.Value = castedValue;
                return;
            }

            try
            {
                this.Value = (T) Convert.ChangeType(value, typeof(T));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($@"Can't convert the value of ""{typeof(TRequested)}"" type into the field value of ""{typeof(T)}"" type", ex);
            }
        }
    }
}
