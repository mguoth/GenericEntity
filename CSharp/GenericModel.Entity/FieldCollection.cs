using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GenericModel.Entity.Abstractions;

namespace GenericModel.Entity
{
    public class FieldCollection : IEnumerable<IField>
    {
        private IDictionary<string, IField> fields = new Dictionary<string, IField>();

        public FieldCollection(IEnumerable<IField> fields)
        {
            this.fields = fields.ToDictionary(x => x.Name);
        }

        /// <summary>
        /// Gets the <see cref="IField" /> with the specified name.
        /// </summary>
        /// <value>
        /// The <see cref="IField"/>.
        /// </value>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">@"The field name ""{fieldName}"" doesn't exist in the field collection")</exception>
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

        /// <summary>
        /// Gets the <see cref="IField" /> with the specified name.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="field">When this method returns, the field associated with the specified name, if the name is found; otherwise null.</param>
        /// <returns>
        /// true in case the <see cref="IField" /> with specified name exists; otherwise false
        /// </returns>
        public bool TryGetField(string fieldName, out IField field)
        {
            return this.fields.TryGetValue(fieldName, out field);
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
