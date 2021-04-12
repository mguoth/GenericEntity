using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity.Abstractions
{
    public class GenericAccess<T> : IGenericAccess<T>
    {
        private readonly IField field;

        public GenericAccess(IField field)
        {
            this.field = field;
        }

        /// <inheritdoc/>
        public bool IsGetSafe
        {
            get
            {
                return field is IGetter<T>;
            }
        }

        /// <inheritdoc/>
        public bool IsSetSafe
        {
            get
            {
                return field is ISetter<T>;
            }
        }

        /// <inheritdoc/>
        public T Value
        {
            get
            {
                IGetter<T> implicitGetter = field as IGetter<T>;
                if (implicitGetter != null)
                {
                    return implicitGetter.Value;
                }

                //Try convert
                return (T) Convert.ChangeType(field.AsRaw().Value, typeof(T));
            }
            set
            {
                ISetter<T> implicitSetter = field as ISetter<T>;
                if (implicitSetter != null)
                {
                    implicitSetter.Value = value;
                    return;
                }

                //Try convert
                field.AsRaw().Value = Convert.ChangeType(value, field.DataType);
            }
        }
    }
}
