using System;

namespace GenericEntity.Abstractions
{
    /// <summary>
    /// Base field class
    /// </summary>
    public abstract class Field : IField
    {
        public Field(IFieldDefinition definition)
        {
            this.Definition = definition;
        }

        /// <inheritdoc/>
        public IFieldDefinition Definition { get; }

        /// <inheritdoc/>
        public Type DataType
        {
            get
            {
                return this.GetDataTypeInternal();
            }
        }

        /// <inheritdoc/>
        public T Get<T>()
        {
            if (this is IField<T> field)
            {
                return field.Value;
            }

            //Try convert
            try
            {
                return (T)Convert.ChangeType(this.GetValueInternal(), typeof(T));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($@"Can't convert the field value of ""{this.DataType}"" type into the ""{typeof(T)}"" return type", ex);
            }
        }

        /// <inheritdoc/>
        public void Set<T>(T value)
        {
            this.SetValueInternal(value);
        }

        protected abstract Type GetDataTypeInternal();
        protected abstract void SetValueInternal<T>(T Value);
        protected abstract object GetValueInternal();
    }

    /// <summary>
    /// Generic base field class
    /// </summary>
    /// <typeparam name="T">The field value type</typeparam>
    public abstract class Field<T> : Field, IField<T>
    {
        public Field(IFieldDefinition definition) : base(definition)
        {
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public T Value { get; set; }

        protected sealed override Type GetDataTypeInternal()
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
