using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity.Abstractions
{
    public class NotSupportedGetter<T> : IGetter<T>
    {
        private readonly IField field;

        public NotSupportedGetter(IField field)
        {
            this.field = field;
        }

        public T Value
        {
            get
            {
                throw new NotSupportedException($@"The field ""{this.field.Definition.Name}"" of ""{this.field.DataType}"" data type doesn't support ""{typeof(IGetter<T>)}""");
            }
        }

        /// <inheritdoc/>
        public bool IsSupported => false;
    }
}
