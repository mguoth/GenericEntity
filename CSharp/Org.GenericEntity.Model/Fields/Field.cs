using Org.GenericEntity.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Org.GenericEntity.Model
{
    /// <summary>
    /// Generic field class
    /// </summary>
    /// <typeparam name="T">The field value type</typeparam>
    internal class Field<T> : Field, IField<T>
    {
        public Field(FieldDefinition fieldDefinition) : base(fieldDefinition)
        {
            this.SetValue(fieldDefinition.DefaultValue);
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
            if (value == null)
            {
                this.Value = (T) (object) value;
                return;
            }

            if (value is T castedValue)
            {
                this.Value = castedValue;
                return;
            }

            try
            {
                this.Value = (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($@"Can't convert the value of ""{typeof(TRequested)}"" type into the field value of ""{typeof(T)}"" type", ex);
            }
        }
    }
}
