using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity.Abstractions
{
    public class NotSupportedSetter<T> : ISetter<T>
    {
        private readonly IField field;

        public NotSupportedSetter(IField field)
        {
            this.field = field;
        }

        public T Value
        {
            set
            {
                throw new NotSupportedException($@"The field ""{this.field.Definition.Name}"" of ""{this.field.DataType}"" data type doesn't support ""{typeof(ISetter<T>)}""");
            }
        }

        /// <inheritdoc/>
        public bool IsSupported => false;
    }
}
