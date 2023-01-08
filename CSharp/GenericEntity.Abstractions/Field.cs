using System;

namespace GenericEntity.Abstractions
{
    /// <summary>
    /// Base field class
    /// </summary>
    public abstract class Field : IField
    {
        public Field(FieldDefinition fieldDefinition)
        {
            this.RawSchema = fieldDefinition.RawSchema;
            this.Name = fieldDefinition.Name;
            this.DisplayName = fieldDefinition.DisplayName;
            this.Description = fieldDefinition.Description;
            this.Nullable = fieldDefinition.Nullable;
        }

        /// <inheritdoc/>
        public string RawSchema { get; }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public string DisplayName { get; }

        /// <inheritdoc/>
        public string Description { get; }

        /// <inheritdoc/>
        public Type ValueType
        {
            get
            {
                return this.GetValueTypeInternal();
            }
        }

        /// <inheritdoc/>
        public bool Nullable { get; }

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
            if (value == null && !Nullable)
            {
                throw new InvalidOperationException($@"Can't set null into not nullable field");
            }

            this.SetValueInternal(value);
        }

        protected abstract Type GetValueTypeInternal();
        protected abstract void SetValueInternal<TSource>(TSource Value);
        protected abstract object GetValueInternal();
    }
}
