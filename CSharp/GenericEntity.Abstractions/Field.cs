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

        private object Value
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
    public abstract class Field<T> : Field, IGetter<T>, ISetter<T>, IGetterSetterSupported
    {
        public Field(IFieldDefinition definition) : base(definition)
        {
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        protected T Value { get; set; }

        /// <inheritdoc/>
        T IGetter<T>.Value
        {
            get
            {
                return this.Value;
            }
        }

        /// <inheritdoc/>
        T ISetter<T>.Value
        {
            set
            {
                this.Value = value;
            }
        }

        /// <inheritdoc/>
        bool IGetterSetterSupported.IsSupported => true;

        protected sealed override Type GetDataTypeInternal()
        {
            return typeof(T);
        }

        protected sealed override object GetValueInternal()
        {
            return ((IGetter<T>) this).Value;
        }

        protected sealed override void SetValueInternal(object value)
        {
            try
            {
                ((ISetter<T>) this).Value = (T)value;
            }
            catch (InvalidCastException ex)
            {
                throw new InvalidOperationException($@"Can't set a value of ""{value.GetType()}"" type into the field value of ""{this.DataType}"" type", ex);
            }
        }
    }
}
