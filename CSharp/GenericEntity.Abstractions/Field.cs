using System;

namespace GenericEntity.Abstractions
{
    /// <summary>
    /// Base field class
    /// </summary>
    public abstract class Field : IField, IRawAccess
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

        object IRawAccess.Value
        {
            get
            {
                return this.GetValueInternal();
            }
            set
            {
                this.SetValueInternal(value);
            }
        }
        
        protected abstract Type GetDataTypeInternal();
        protected abstract void SetValueInternal(object Value);
        protected abstract object GetValueInternal();
    }

    /// <summary>
    /// Generic base field class
    /// </summary>
    /// <typeparam name="T">The field value type</typeparam>
    public abstract class Field<T> : Field, IGenericAccess<T>
    {
        public Field(IFieldDefinition definition) : base(definition)
        {
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        protected T Value { get; set; }

        /// <inheritdoc/>
        T IGenericAccess<T>.Value
        {
            get
            {
                return this.Value;
            }
            set
            {
                this.Value = value;
            }
        }

        /// <inheritdoc/>
        bool IGenericAccess<T>.IsGetSafe => true;

        /// <inheritdoc/>
        bool IGenericAccess<T>.IsSetSafe => true;

        protected sealed override Type GetDataTypeInternal()
        {
            return typeof(T);
        }

        protected sealed override object GetValueInternal()
        {
            return this.Value;
        }

        protected sealed override void SetValueInternal(object value)
        {
            try
            {
                ((IGenericAccess<T>) this).Value = (T)value;
            }
            catch (InvalidCastException ex)
            {
                throw new InvalidOperationException($@"Can't set a value of ""{value.GetType()}"" type into the field value of ""{this.DataType}"" type", ex);
            }
        }
    }
}
