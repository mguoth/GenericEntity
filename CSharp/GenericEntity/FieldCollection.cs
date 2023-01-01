using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GenericEntity.Abstractions;

namespace GenericEntity
{
    public class FieldCollection : IEnumerable<IField>
    {
        private IDictionary<string, IField> fields = new Dictionary<string, IField>();

        public FieldCollection(IEnumerable<IField> fields)
        {
            this.fields = fields.ToDictionary(x => x.Name);
        }

        /// <summary>
        /// Gets the <see cref="IField"/> with the specified field name.
        /// </summary>
        /// <value>
        /// The <see cref="IField"/>.
        /// </value>
        /// <param name="fieldName">Name of the field. Use field names from schema (avoid hardcoded values)</param>
        /// <returns></returns>
        public IField this[string fieldName]
        {
            get
            {
                IField field;
                if (this.fields.TryGetValue(fieldName, out field))
                {
                    return field;
                }
                
                throw new KeyNotFoundException($@"The field name ""{fieldName}"" doesn't exist in the field collection");
            }
        }

        public IEnumerator<IField> GetEnumerator()
        {
            return this.fields.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
